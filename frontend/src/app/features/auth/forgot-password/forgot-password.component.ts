import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../core/auth/services/auth.service';

@Component({
  selector: 'app-forgot-password',
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
    <section class="forgot" aria-label="Forgot password form">
      @if (!submitted()) {
        <div class="forgot__header">
          <h1>Forgot password?</h1>
          <p class="text-muted">Enter your email and we'll send you a reset link.</p>
        </div>

        <form [formGroup]="form" (ngSubmit)="onSubmit()" class="forgot__form" novalidate>
          <mat-form-field>
            <mat-label>Email address</mat-label>
            <input matInput type="email" formControlName="email" autocomplete="email" />
            <mat-error>
              @if (form.get('email')?.hasError('required')) { Email is required }
              @else if (form.get('email')?.hasError('email')) { Enter a valid email address }
            </mat-error>
          </mat-form-field>

          <button
            mat-raised-button
            color="primary"
            type="submit"
            class="forgot__submit"
            [disabled]="isSubmitting()"
          >
            @if (isSubmitting()) { <mat-spinner diameter="20" /> }
            @else { Send Reset Link }
          </button>
        </form>
      } @else {
        <div class="forgot__success" role="status">
          <mat-icon class="forgot__success-icon">mark_email_read</mat-icon>
          <h2>Check your inbox</h2>
          <p>
            We've sent a password reset link to <strong>{{ form.get('email')?.value }}</strong>.
            The link expires in 1 hour.
          </p>
        </div>
      }

      <p class="forgot__back">
        <a routerLink="/auth/login">
          <mat-icon>arrow_back</mat-icon>
          Back to login
        </a>
      </p>
    </section>
  `,
  styleUrl: './forgot-password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ForgotPasswordComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);

  readonly isSubmitting = signal(false);
  readonly submitted = signal(false);

  readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);

    this.authService.forgotPassword(this.form.getRawValue().email).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.submitted.set(true);
      },
      error: () => {
        this.isSubmitting.set(false);
        this.submitted.set(true);
      },
    });
  }
}
