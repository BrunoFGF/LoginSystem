import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Person} from '../../../../../../models/person.model';
import {UserService} from '../../../../../../services/user.service';
import {PersonService} from '../../../../../../services/person.service';
import {ActivatedRoute, Router} from '@angular/router';
import {UserRequest} from '../../../../../../models/user.model';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  standalone: true,
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  userId: number | null = null;
  isEditMode: boolean = false;
  loading: boolean = false;
  persons: Person[] = [];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private personService: PersonService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      personId: ['', Validators.required],
      status: ['Active']
    });
  }

  ngOnInit(): void {
    this.loadPersons();

    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.userId = +id;
      this.isEditMode = true;
      this.loadUserData(this.userId);

      // Remove password validator in edit mode
      this.userForm.get('password')?.setValidators(null);
      this.userForm.get('password')?.updateValueAndValidity();
    }
  }

  loadPersons(): void {
    this.personService.listSelectPersons().subscribe(
      response => {
        if (response.isSuccess) {
          this.persons = response.data as Person[];
        }
      },
      error => console.error('Error loading persons', error)
    );
  }

  loadUserData(userId: number): void {
    this.loading = true;
    this.userService.getUserById(userId).subscribe(
      response => {
        if (response.isSuccess) {
          const user = response.data;
          this.userForm.patchValue({
            username: user.username,
            personId: user.personId,
            status: user.status
          });
          // Don't populate password field for security reasons
        }
        this.loading = false;
      },
      error => {
        console.error('Error loading user data', error);
        this.loading = false;
      }
    );
  }

  onSubmit(): void {
    if (this.userForm.invalid) {
      return;
    }

    const userData: UserRequest = this.userForm.value;
    this.loading = true;

    if (this.isEditMode && this.userId) {
      // If password is empty in edit mode, remove it from request
      /*if (!userData.password) {
        delete userData.password;
      }*/

      this.userService.editUser(this.userId, userData).subscribe(
        response => {
          this.loading = false;
          if (response.isSuccess) {
            this.router.navigate(['/dashboard/admin/users']);
          }
        },
        error => {
          console.error('Error updating user', error);
          this.loading = false;
        }
      );
    } else {
      this.userService.registerUser(userData).subscribe(
        response => {
          this.loading = false;
          if (response.isSuccess) {
            this.router.navigate(['/dashboard/admin/users']);
          }
        },
        error => {
          console.error('Error creating user', error);
          this.loading = false;
        }
      );
    }
  }
}
