import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  submitted = false;
  passwordRetrieved = false;
  retrievedPassword: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.forgotPasswordForm = this.formBuilder.group({
      documentId: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {}

  get f() { return this.forgotPasswordForm.controls; }

  onSubmit() {
    this.submitted = true;

    if (this.forgotPasswordForm.invalid) {
      return;
    }

    const recoveryData = {
      documentId: this.f['documentId'].value,
      email: this.f['email'].value
    };

    this.authService.recoverPassword(recoveryData).subscribe({
      next: (response) => {
        this.retrievedPassword = response.password;
        this.passwordRetrieved = true;
        alert('Contraseña recuperada con éxito');
      },
      error: (error) => {
        alert('Error al recuperar la contraseña: ' + error.message);
      }
    });
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}
