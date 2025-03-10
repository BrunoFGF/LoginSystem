import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';
import {AccountRequest, AccountResponse} from '../../../models/account/account.model';
import {AccountService} from '../../../core/services/account.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  accountData: AccountResponse | null = null;
  loading: boolean = false;
  success: boolean = false;
  error: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private accountService: AccountService
  ) {
    this.profileForm = this.formBuilder.group({
      username: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      identityCard: ['', Validators.required],
      birthDate: ['', Validators.required],
      password: [''],
      confirmPassword: ['']
    });
  }

  ngOnInit(): void {
    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.loading = true;
    this.accountService.getAccount().subscribe({
      next: (data) => {
        this.accountData = data;

        this.profileForm.patchValue({
          username: data.username,
          firstName: data.person?.firstName || '',
          lastName: data.person?.lastName || '',
          identityCard: data.person?.identityCard || '',
          birthDate: data.person?.birthDate ? this.formatDateForInput(data.person.birthDate) : ''
        });

        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        this.error = 'Error al cargar el perfil: ' + (error.error?.message || 'Desconocido');
        console.error('Error cargando perfil:', error);
      }
    });
  }

  formatDateForInput(dateString: string): string {
    if (!dateString) return '';

    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      return;
    }

    this.loading = true;
    this.success = false;
    this.error = '';

    if (this.profileForm.get('password')?.value !== this.profileForm.get('confirmPassword')?.value) {
      this.error = 'Las contraseñas no coinciden';
      this.loading = false;
      return;
    }

    const requestData: AccountRequest = {
      username: this.profileForm.get('username')?.value,
      firstName: this.profileForm.get('firstName')?.value,
      lastName: this.profileForm.get('lastName')?.value,
      identityCard: this.profileForm.get('identityCard')?.value,
      birthDate: this.profileForm.get('birthDate')?.value,
    };

    const password = this.profileForm.get('password')?.value;
    if (password) {
      requestData.password = password;
    }

    this.accountService.updateAccount(requestData).subscribe({
      next: (response) => {
        this.loading = false;
        this.success = true;

        if (this.accountData && this.accountData.username !== requestData.username) {
          this.authService.updateUsername(requestData.username);
        }
      },
      error: (error) => {
        this.loading = false;
        this.error = 'Error al actualizar el perfil: ' + (error.error?.message || 'Desconocido');
        console.error('Error actualizando perfil:', error);
      }
    });
  }
}
