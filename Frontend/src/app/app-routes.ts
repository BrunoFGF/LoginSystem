import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { AuthGuard } from './core/guards/auth.guard';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LayoutComponent } from './layout/main-layout/layout.component';
import { UsersListComponent } from './pages/users-list/users-list.component';
import { UserFormComponent } from './pages/admin/user-form/user-form.component';
import { RoleGuard } from './core/guards/role.guard';
import {ProfileComponent} from './shared/components/profile/profile.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'profile', component: ProfileComponent },
      {
        path: 'admin/users',
        component: UsersListComponent,
        canActivate: [RoleGuard],
        data: { role: 'ADMIN' }
      },
      {
        path: 'admin/users/new',
        component: UserFormComponent,
        canActivate: [RoleGuard],
        data: { role: 'ADMIN' }
      },
      {
        path: 'admin/users/edit/:id',
        component: UserFormComponent,
        canActivate: [RoleGuard],
        data: { role: 'ADMIN' }
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: '/login' }
];
