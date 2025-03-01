import { Component, OnInit } from '@angular/core';
import {MenuItem} from '../../../../models/menu.model';
import {SidebarComponent} from '../../../../layout/components/sidebar/sidebar.component';
import {HeaderComponent} from '../../../../layout/components/header/header.component';
import {FooterComponent} from '../../../../layout/components/footer/footer.component';
import {RouterOutlet} from '@angular/router';


@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  standalone: true,
  imports: [
    SidebarComponent,
    HeaderComponent,
    FooterComponent,
    RouterOutlet
  ],
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  menuItems: MenuItem[] = [
    { id: 1, name: 'Usuarios', icon: 'person', url: '/dashboard/admin/users', roles: ['admin'] },
    { id: 2, name: 'Personas', icon: 'people', url: '/dashboard/admin/persons', roles: ['admin'] }
  ];

  constructor() { }

  ngOnInit(): void {
  }
}
