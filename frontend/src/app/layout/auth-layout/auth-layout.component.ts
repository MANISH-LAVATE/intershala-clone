import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-auth-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <div class="auth-layout">
      <div class="auth-layout__brand">
        <a href="/" class="auth-layout__logo">
          <img src="/assets/icons/logo.svg" alt="Internshala Clone" width="140" height="36" />
        </a>
        <div class="auth-layout__tagline">
          <h1>Start your career journey today</h1>
          <p>Join millions of students and employers on India's most trusted internship platform.</p>
        </div>
      </div>
      <div class="auth-layout__card">
        <router-outlet />
      </div>
    </div>
  `,
  styleUrl: './auth-layout.component.scss',
})
export class AuthLayoutComponent {}
