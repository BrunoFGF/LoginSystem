import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PersonFormComponent} from './components/person-management/person-form/person-form.component';
import {PersonListComponent} from './components/person-management/person-list/person-list.component';
import {UserFormComponent} from './components/user-management/user-form/user-form.component';
import {UserListComponent} from './components/user-management/user-list/user-list.component';
import {AdminDashboardComponent} from './admin-dashboard/admin-dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: AdminDashboardComponent,
    // canActivate: [RoleGuard],
    // data: { roles: ['admin'] },
    children: [
      { path: 'users', component: UserListComponent },
      { path: 'users/new', component: UserFormComponent },
      { path: 'users/:id', component: UserFormComponent },
      { path: 'persons', component: PersonListComponent },
      { path: 'persons/new', component: PersonFormComponent },
      { path: 'persons/:id', component: PersonFormComponent },
      { path: '', redirectTo: 'users', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminDashboardRoutingModule { }
