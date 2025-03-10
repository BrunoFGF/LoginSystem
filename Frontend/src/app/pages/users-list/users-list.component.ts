import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {Router, RouterModule} from '@angular/router';
import { UserService } from '../../core/services/user.service';
import {AuthService} from '../../core/services/auth.service';
import {UserResponse} from '../../models/user/user.model';
import {BaseFiltersRequest} from '../../models/user/filters.model';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  users: UserResponse[] = [];
  totalRecords: number = 0;
  loading: boolean = false

  Math = Math;

  filters: BaseFiltersRequest = {
    numPage: 1,
    numRecordsPage: 10,
    order: 'asc',
    sort: 'username',
    numFilter: 1,
    textFilter: ''
  };

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    if (!this.authService.isAdmin()) {
      this.router.navigate(['/dashboard']);
      return;
    }

    const token = this.authService.getToken();

    if (token && this.authService.isLoggedIn()) {
      this.loadUsers();
    } else {
      this.authService.logout();
    }
  }

  loadUsers(): void {
    this.loading = true;

    this.userService.listUsers(this.filters).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.users = response.data.items || response.data.items || [];
          this.totalRecords = response.data.totalRecords || response.data.totalRecords || 0;
        }
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;

        if (error.status !== 401) {
          alert('Error al cargar los usuarios. Por favor, intenta de nuevo más tarde.');
        }
      }
    });
  }

  search(): void {
    this.filters.numPage = 1;
    this.loadUsers();
  }

  clearSearch(): void {
    this.filters.textFilter = '';
    this.search();
  }

  pageChanged(page: number): void {
    this.filters.numPage = page;
    this.loadUsers();
  }

  changePageSize(size: number): void {
    this.filters.numRecordsPage = size;
    this.filters.numPage = 1;
    this.loadUsers();
  }

  deleteUser(userId: number): void {
    if (confirm('¿Está seguro de eliminar este usuario?')) {
      this.userService.removeUser(userId).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            alert('Usuario eliminado correctamente');
            this.loadUsers();
          } else {
            alert(`Error al eliminar usuario: ${response.message}`);
          }
        },
        error: (error) => {
          console.error('Error al eliminar usuario:', error);
          alert('Ocurrió un error al eliminar el usuario');
        }
      });
    }
  }

  getPages(): number[] {
    const totalPages = Math.ceil(this.totalRecords / this.filters.numRecordsPage);
    const currentPage = this.filters.numPage;
    const pages: number[] = [];

    if (totalPages <= 5) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      if (currentPage <= 3) {
        for (let i = 1; i <= 5; i++) {
          pages.push(i);
        }
      } else if (currentPage >= totalPages - 2) {
        for (let i = totalPages - 4; i <= totalPages; i++) {
          pages.push(i);
        }
      } else {
        for (let i = currentPage - 2; i <= currentPage + 2; i++) {
          pages.push(i);
        }
      }
    }

    return pages;
  }
}
