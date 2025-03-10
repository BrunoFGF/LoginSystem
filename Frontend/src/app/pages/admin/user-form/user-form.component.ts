import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  userId: number | null = null;
  isEditMode: boolean = false;
  loading: boolean = false;
  errorMessage: string = '';
  roles: string[] = ['ADMIN', 'USER'];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(80)]],
      lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(80)]],
      identityCard: ['', [Validators.required, Validators.maxLength(10), Validators.pattern('^(?!.*(\\d)\\1{3})[0-9]{10}$')]],
      birthDate: [null],
      username: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(20),
        Validators.pattern(/^(?!.*[\W_])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$/)
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(16),
        Validators.pattern(/^(?!.*\s)(?=.*[A-Z])(?=.*[\W_]).{8,16}$/)
      ]],
      status: ['Activo'],
      rolName: ['USER', Validators.required]
    });
  }

  passwordVisible: boolean = false;

  togglePasswordVisibility(): void {
    this.passwordVisible = !this.passwordVisible;
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.userId = +id;
      this.isEditMode = true;
      this.loadUserData();
    }
  }

  loadUserData(): void {
    if (!this.userId) return;

    this.loading = true;
    this.userService.getUserById(this.userId).subscribe({
      next: (response) => {

        if (response.isSuccess && response.data) {
          const user = response.data;

          if (this.isEditMode) {
            this.userForm.get('password')?.clearValidators();
            this.userForm.get('password')?.setValidators([
              Validators.minLength(8),
              Validators.maxLength(16),
              Validators.pattern(/^(?!.*\s)(?=.*[A-Z])(?=.*[\W_]).{8,16}$/)
            ]);
            this.userForm.get('password')?.updateValueAndValidity();
          }

          let birthDate = null;
          if (user.person?.birthDate) {
            birthDate = new Date(user.person.birthDate);
            if (isNaN(birthDate.getTime())) {
              birthDate = null;
            } else {
              const year = birthDate.getFullYear();
              const month = String(birthDate.getMonth() + 1).padStart(2, '0');
              const day = String(birthDate.getDate()).padStart(2, '0');
              birthDate = `${year}-${month}-${day}`;
            }
          }

          this.userForm.patchValue({
            firstName: user.person?.firstName || '',
            lastName: user.person?.lastName || '',
            identityCard: user.person?.identityCard || '',
            birthDate: birthDate,
            //password: user.password || '',
            username: user.username || '',
            status: user.status || 'Active',
            rolName: user.rolName || 'USER'
          });
        } else {
          this.errorMessage = response.message || 'Error al cargar datos del usuario';
        }
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'Ocurrió un error al cargar los datos del usuario';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }

    const formValues = this.userForm.value;
    if (formValues.birthDate) {
      const birthDate = new Date(formValues.birthDate);
      if (!isNaN(birthDate.getTime())) {
        birthDate.toISOString();
      }
    }

    const userData = {
      firstName: formValues.firstName,
      lastName: formValues.lastName,
      identityCard: formValues.identityCard,
      birthDate: formValues.birthDate ? formValues.birthDate : null,
      username: formValues.username,
      password: formValues.password || undefined,
      status: formValues.status || 'Active',
      rolName: formValues.rolName || 'USER'
    };

    console.log('Datos a enviar:', userData);
    this.loading = true;

    if (this.isEditMode && this.userId) {
      if (!userData.password) {
        delete userData.password;
      }

      this.userService.editUser(this.userId, userData).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            alert('Usuario actualizado correctamente');
            this.router.navigate(['/admin/users']);
          } else {
            this.errorMessage = response.message || 'Error al actualizar usuario';
          }
          this.loading = false;
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Ocurrió un error al actualizar el usuario';
          this.loading = false;
        }
      });
    } else {
      this.userService.registerUser(userData).subscribe({
        next: (response) => {
          if (response.isSuccess) {
            alert('Usuario creado correctamente');
            this.router.navigate(['/admin/users']);
          } else {
            this.errorMessage = response.message || 'Error al crear usuario';
          }
          this.loading = false;
        },
        error: (error) => {
          this.errorMessage = 'Ocurrió un error al crear el usuario';
          this.loading = false;
        }
      });
    }
  }

  hasError(controlName: string, errorName: string): boolean {
    const control = this.userForm.get(controlName);
    return !!control && control.hasError(errorName) && (control.dirty || control.touched);
  }

  getErrorMessage(controlName: string): string {
    const control = this.userForm.get(controlName);
    if (!control) return '';

    if (control.hasError('required')) return 'Este campo es obligatorio';
    if (control.hasError('minlength')) {
      const minLength = control.getError('minlength').requiredLength;
      return `Debe tener al menos ${minLength} caracteres`;
    }
    if (control.hasError('maxlength')) {
      const maxLength = control.getError('maxlength').requiredLength;
      return `No puede tener más de ${maxLength} caracteres`;
    }
    if (control.hasError('pattern')) {
      if (controlName === 'username') {
        return 'El nombre de usuario debe contener al menos una letra mayúscula, un número y no debe incluir signos';
      }
      if (controlName === 'password') {
        return 'La contraseña debe tener al menos una letra mayúscula, un signo y no debe contener espacios';
      }
      if (controlName === 'identityCard') {
        return 'El documento debe contener solo números y no puede tener un mismo dígito repetido más de 3 veces seguidas';
      }
    }
    return 'Campo inválido';
  }

  onCancel(): void {
    this.router.navigate(['/admin/users']);
  }
}
