import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  roles: string[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Output() collapsedChange = new EventEmitter<boolean>();

  menuItems: MenuItem[] = [];
  userRole: string = '';
  isCollapsed: boolean = false;

  constructor(public authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.userRole = this.authService.getUserRole();
        this.loadMenuItems();
      }
    });
  }

  loadMenuItems(): void {
    const commonMenuItems: MenuItem[] = [
      {
        label: 'Dashboard',
        icon: 'fa-tachometer-alt',
        route: '/dashboard',
        roles: ['ADMIN', 'USER']
      },
      {
        label: 'Mi Perfil',
        icon: 'fa-user',
        route: '/profile',
        roles: ['ADMIN', 'USER']
      },
    ];

    const adminMenuItems: MenuItem[] = [
      {
        label: 'Gestión de Usuarios',
        icon: 'fa-users',
        route: '/admin/users',
        roles: ['ADMIN']
      },
    ];

    const userMenuItems: MenuItem[] = [
    ];

    this.menuItems = [...commonMenuItems, ...adminMenuItems, ...userMenuItems]
      .filter(item => item.roles.includes(this.userRole));
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
    this.collapsedChange.emit(this.isCollapsed);
  }

  logout(): void {
    this.authService.logout();
  }
}
