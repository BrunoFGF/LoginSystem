import {Component, OnInit} from '@angular/core';
import {Person} from '../../../../../../models/person.model';
import {BaseFiltersRequest} from '../../../../../../models/filter.model';
import {PersonService} from '../../../../../../services/person.service';
import {SearchComponent} from '../../../../../../shared/components/search/search.component';
import {DatePipe} from '@angular/common';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  standalone: true,
  imports: [
    SearchComponent,
    DatePipe,
    RouterLink
  ],
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent implements OnInit {
  persons: Person[] = [];
  loading: boolean = false;
  filters: BaseFiltersRequest = {
    pageNum: 1,
    pageSize: 10
  };
  totalItems: number = 0;

  constructor(private personService: PersonService) { }

  ngOnInit(): void {
    this.loadPersons();
  }

  loadPersons(): void {
    this.loading = true;
    this.personService.listPersons(this.filters).subscribe(
      response => {
        if (response.isSuccess) {
          this.persons = response.data as Person[];
          this.totalItems = Array.isArray(response.data) ? response.data.length : 0;
        }
        this.loading = false;
      },
      error => {
        console.error('Error loading persons', error);
        this.loading = false;
      }
    );
  }

  onSearch(textFilter: string): void {
    this.filters.textFilter = textFilter;
    this.filters.pageNum = 1;
    this.loadPersons();
  }

  onPageChange(page: number): void {
    this.filters.pageNum = page;
    this.loadPersons();
  }

  deletePerson(personId: number): void {
    if (confirm('¿Está seguro de eliminar esta persona?')) {
      this.personService.removePerson(personId).subscribe(
        response => {
          if (response.isSuccess) {
            this.loadPersons();
          }
        },
        error => console.error('Error deleting person', error)
      );
    }
  }
}
