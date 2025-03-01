import { Component, OnInit } from '@angular/core';
import {User, UserResponse} from '../../../../../../models/user.model';
import {BaseFiltersRequest} from '../../../../../../models/filter.model';
import {UserService} from '../../../../../../services/user.service';
import {SearchComponent} from '../../../../../../shared/components/search/search.component';
import {DatePipe} from '@angular/common';
import {RouterLink} from '@angular/router';


@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  standalone: true,
  imports: [
    SearchComponent,
    DatePipe,
    RouterLink
  ],
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  loading: boolean = false;
  filters: BaseFiltersRequest = {
    pageNum: 1,
    pageSize: 10
  };
  totalItems: number = 0;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.userService.listUsers(this.filters).subscribe(
      (response: UserResponse) => {
        if (response.isSuccess) {
          this.users = response.data as User[];
          this.totalItems = Array.isArray(response.data) ? response.data.length : 0;
        }
        this.loading = false;
      },
      error => {
        console.error('Error al cargar usuarios', error);
        this.loading = false;
      }
    );
  }

  onSearch(textFilter: string): void {
    this.filters.textFilter = textFilter;
    this.filters.pageNum = 1;
    this.loadUsers();
  }

  onPageChange(page: number): void {
    this.filters.pageNum = page;
    this.loadUsers();
  }

  deleteUser(userId: number): void {
    if (confirm('¿Está seguro de eliminar este usuario?')) {
      this.userService.removeUser(userId).subscribe(
        response => {
          if (response.isSuccess) {
            this.loadUsers();
          }
        },
        error => console.error('Error al borrar usuario', error)
      );
    }
  }
}
