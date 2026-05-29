import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import {
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/auth/services/auth.service';

function passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
  const parent = control.parent;
  if (!parent) return null;
  const password = parent.get('password')?.value as string;
  return control.value === password ? null : { passwordMismatch: true };
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatTabsModule,
  ],
  template: `
    <section class="register" aria-label="Registration form">
      <div class="register__header">
        <h1 class="register__title">Create account</h1>
        <p class="text-muted">Join Internshala Clone today</p>
      </div>

      <mat-tab-group
        [(selectedIndex)]="activeTab"
        animationDuration="200ms"
        class="register__tabs"
      >
        <!-- Student Tab -->
        <mat-tab label="Student">
          <form
            [formGroup]="studentForm"
            (ngSubmit)="onStudentSubmit()"
            class="register__form"
            novalidate
          >
            <div class="register__row">
              <mat-form-field>
                <mat-label>First name</mat-label>
                <input matInput formControlName="firstName" autocomplete="given-name" />
                <mat-error>
                  @if (studentForm.get('firstName')?.hasError('required')) { First name is required }
                  @else if (studentForm.get('firstName')?.hasError('minlength')) { Minimum 2 characters }
                </mat-error>
              </mat-form-field>

              <mat-form-field>
                <mat-label>Last name</mat-label>
                <input matInput formControlName="lastName" autocomplete="family-name" />
                <mat-error>
                  @if (studentForm.get('lastName')?.hasError('required')) { Last name is required }
                </mat-error>
              </mat-form-field>
            </div>

            <mat-form-field>
              <mat-label>Email address</mat-label>
              <input matInput type="email" formControlName="email" autocomplete="email" />
              <mat-error>
                @if (studentForm.get('email')?.hasError('required')) { Email is required }
                @else if (studentForm.get('email')?.hasError('email')) { Enter a valid email }
              </mat-error>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Password</mat-label>
              <input
                matInput
                [type]="showPassword() ? 'text' : 'password'"
                formControlName="password"
                autocomplete="new-password"
              />
              <button matSuffix mat-icon-button type="button" (click)="togglePassword()">
                <mat-icon>{{ showPassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
              </button>
              <mat-hint>Min 8 chars, 1 uppercase, 1 number, 1 special character</mat-hint>
              <mat-error>
                @if (studentForm.get('password')?.hasError('required')) { Password is required }
                @else if (studentForm.get('password')?.hasError('pattern')) { Password does not meet requirements }
              </mat-error>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Confirm password</mat-label>
              <input
                matInput
                [type]="showPassword() ? 'text' : 'password'"
                formControlName="confirmPassword"
                autocomplete="new-password"
              />
              <mat-error>
                @if (studentForm.get('confirmPassword')?.hasError('required')) { Please confirm your password }
                @else if (studentForm.get('confirmPassword')?.hasError('passwordMismatch')) { Passwords do not match }
              </mat-error>
            </mat-form-field>

            @if (errorMessage()) {
              <div class="register__error" role="alert">
                <mat-icon>error_outline</mat-icon>
                {{ errorMessage() }}
              </div>
            }

            <button
              mat-raised-button
              color="primary"
              type="submit"
              class="register__submit"
              [disabled]="isSubmitting()"
            >
              @if (isSubmitting()) { <mat-spinner diameter="20" /> }
              @else { Create Student Account }
            </button>
          </form>
        </mat-tab>

        <!-- Employer Tab -->
        <mat-tab label="Employer">
          <form
            [formGroup]="employerForm"
            (ngSubmit)="onEmployerSubmit()"
            class="register__form"
            novalidate
          >
            <div class="register__row">
              <mat-form-field>
                <mat-label>First name</mat-label>
                <input matInput formControlName="firstName" autocomplete="given-name" />
                <mat-error>
                  @if (employerForm.get('firstName')?.hasError('required')) { Required }
                </mat-error>
              </mat-form-field>

              <mat-form-field>
                <mat-label>Last name</mat-label>
                <input matInput formControlName="lastName" autocomplete="family-name" />
                <mat-error>
                  @if (employerForm.get('lastName')?.hasError('required')) { Required }
                </mat-error>
              </mat-form-field>
            </div>

            <mat-form-field>
              <mat-label>Company name</mat-label>
              <input matInput formControlName="companyName" autocomplete="organization" />
              <mat-error>
                @if (employerForm.get('companyName')?.hasError('required')) { Company name is required }
              </mat-error>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Work email</mat-label>
              <input matInput type="email" formControlName="email" autocomplete="email" />
              <mat-error>
                @if (employerForm.get('email')?.hasError('required')) { Email is required }
                @else if (employerForm.get('email')?.hasError('email')) { Enter a valid email }
              </mat-error>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Password</mat-label>
              <input
                matInput
                [type]="showPassword() ? 'text' : 'password'"
                formControlName="password"
                autocomplete="new-password"
              />
              <button matSuffix mat-icon-button type="button" (click)="togglePassword()">
                <mat-icon>{{ showPassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
              </button>
              <mat-error>
                @if (employerForm.get('password')?.hasError('required')) { Password is required }
                @else if (employerForm.get('password')?.hasError('pattern')) { Password does not meet requirements }
              </mat-error>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Confirm password</mat-label>
              <input
                matInput
                [type]="showPassword() ? 'text' : 'password'"
                formControlName="confirmPassword"
                autocomplete="new-password"
              />
              <mat-error>
                @if (employerForm.get('confirmPassword')?.hasError('passwordMismatch')) { Passwords do not match }
              </mat-error>
            </mat-form-field>

            @if (errorMessage()) {
              <div class="register__error" role="alert">
                <mat-icon>error_outline</mat-icon>
                {{ errorMessage() }}
              </div>
            }

            <button
              mat-raised-button
              color="primary"
              type="submit"
              class="register__submit"
              [disabled]="isSubmitting()"
            >
              @if (isSubmitting()) { <mat-spinner diameter="20" /> }
              @else { Create Employer Account }
            </button>
          </form>
        </mat-tab>
      </mat-tab-group>

      <p class="register__login">
        Already have an account?
        <a routerLink="/auth/login">Sign in</a>
      </p>
    </section>
  `,
  styleUrl: './register.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly isSubmitting = signal(false);
  readonly showPassword = signal(false);
  readonly errorMessage = signal<string | null>(null);
  activeTab = 0;

  togglePassword(): void {
    this.showPassword.update((v) => !v);
  }

  private readonly passwordPattern =
    /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

  readonly studentForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required, Validators.minLength(2)]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.pattern(this.passwordPattern)]],
    confirmPassword: ['', [Validators.required, passwordMatchValidator]],
  });

  readonly employerForm = this.fb.nonNullable.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    companyName: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.pattern(this.passwordPattern)]],
    confirmPassword: ['', [Validators.required, passwordMatchValidator]],
  });

  onStudentSubmit(): void {
    if (this.studentForm.invalid) {
      this.studentForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const { firstName, lastName, email, password, confirmPassword } =
      this.studentForm.getRawValue();

    this.authService
      .registerStudent({ firstName, lastName, email, password, confirmPassword })
      .subscribe({
        next: () => this.router.navigate(['/internships']),
        error: (err) => {
          this.isSubmitting.set(false);
          if (err.status === 409) {
            this.errorMessage.set('An account with this email already exists.');
          } else {
            this.errorMessage.set('Registration failed. Please try again.');
          }
        },
      });
  }

  onEmployerSubmit(): void {
    if (this.employerForm.invalid) {
      this.employerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const { firstName, lastName, email, password, confirmPassword, companyName } =
      this.employerForm.getRawValue();

    this.authService
      .registerEmployer({ firstName, lastName, email, password, confirmPassword, companyName })
      .subscribe({
        next: () => this.router.navigate(['/employer/dashboard']),
        error: (err) => {
          this.isSubmitting.set(false);
          if (err.status === 409) {
            this.errorMessage.set('An account with this email already exists.');
          } else {
            this.errorMessage.set('Registration failed. Please try again.');
          }
        },
      });
  }
}
