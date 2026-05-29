import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../core/auth/services/auth.service';

@Component({
  selector: 'app-login',
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
  ],
  template: `
    <section class="login" aria-label="Login form">
      <div class="login__header">
        <h1 class="login__title">Welcome back</h1>
        <p class="text-muted">Sign in to your Internshala account</p>
      </div>

      <form [formGroup]="form" (ngSubmit)="onSubmit()" class="login__form" novalidate>
        <mat-form-field>
          <mat-label>Email address</mat-label>
          <input
            matInput
            type="email"
            formControlName="email"
            autocomplete="email"
            placeholder="you@example.com"
          />
          <mat-error>
            @if (form.get('email')?.hasError('required')) { Email is required }
            @else if (form.get('email')?.hasError('email')) { Enter a valid email address }
          </mat-error>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Password</mat-label>
          <input
            matInput
            [type]="showPassword() ? 'text' : 'password'"
            formControlName="password"
            autocomplete="current-password"
          />
          <button
            matSuffix
            mat-icon-button
            type="button"
            (click)="togglePassword()"
            [attr.aria-label]="showPassword() ? 'Hide password' : 'Show password'"
          >
            <mat-icon>{{ showPassword() ? 'visibility_off' : 'visibility' }}</mat-icon>
          </button>
          <mat-error>
            @if (form.get('password')?.hasError('required')) { Password is required }
          </mat-error>
        </mat-form-field>

        <div class="login__options">
          <a routerLink="/auth/forgot-password" class="login__forgot">Forgot password?</a>
        </div>

        @if (errorMessage()) {
          <div class="login__error" role="alert">
            <mat-icon>error_outline</mat-icon>
            {{ errorMessage() }}
          </div>
        }

        <button
          mat-raised-button
          color="primary"
          type="submit"
          class="login__submit"
          [disabled]="isSubmitting()"
        >
          @if (isSubmitting()) {
            <mat-spinner diameter="20" />
          } @else {
            Sign In
          }
        </button>
      </form>

      <p class="login__register">
        Don't have an account?
        <a routerLink="/auth/register">Create account</a>
      </p>
    </section>
  `,
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly isSubmitting = signal(false);
  readonly showPassword = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    this.authService.loginStudent(this.form.getRawValue()).subscribe({
      next: () => {
        const returnUrl =
          this.route.snapshot.queryParamMap.get('returnUrl') ?? this.getDefaultRoute();
        this.router.navigateByUrl(returnUrl);
      },
      error: (err) => {
        this.isSubmitting.set(false);
        if (err.status === 401) {
          this.errorMessage.set('Invalid email or password. Please try again.');
        } else if (err.status === 403) {
          this.errorMessage.set('Please verify your email address before logging in.');
        } else {
          this.errorMessage.set('An error occurred. Please try again.');
        }
      },
    });
  }

  togglePassword(): void {
    this.showPassword.update((v) => !v);
  }

  private getDefaultRoute(): string {
    const role = this.authService.currentUser()?.role;
    if (role === 'Employer') return '/employer/dashboard';
    if (role === 'Admin') return '/admin/dashboard';
    return '/internships';
  }
}
