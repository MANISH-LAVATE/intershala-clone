import {
  Component,
  ChangeDetectionStrategy,
  inject,
  output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../core/auth/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatBadgeModule,
    MatDividerModule,
  ],
  template: `
    <mat-toolbar class="header" role="banner">
      <button mat-icon-button class="header__menu-btn" (click)="menuToggled.emit()" aria-label="Toggle navigation menu">
        <mat-icon>menu</mat-icon>
      </button>

      <a routerLink="/" class="header__logo" aria-label="Internshala Clone home">
        <span class="header__logo-text">Internshala</span>
      </a>

      <nav class="header__nav" aria-label="Main navigation">
        <a routerLink="/internships" routerLinkActive="active" mat-button>Internships</a>
        <a routerLink="/jobs" routerLinkActive="active" mat-button>Jobs</a>
        <a routerLink="/courses" routerLinkActive="active" mat-button>Courses</a>
        @if (!authService.isAuthenticated()) {
          <a routerLink="/auth/login" routerLinkActive="active" mat-button>For Employers</a>
        }
      </nav>

      <span class="header__spacer"></span>

      @if (authService.isAuthenticated()) {
        <button mat-icon-button aria-label="Notifications" [matBadge]="'3'" matBadgeColor="warn">
          <mat-icon>notifications_none</mat-icon>
        </button>

        <button mat-icon-button [matMenuTriggerFor]="profileMenu" aria-label="Profile menu">
          <mat-icon>account_circle</mat-icon>
        </button>

        <mat-menu #profileMenu="matMenu">
          <div class="header__profile-info" mat-menu-item disabled>
            <strong>{{ authService.currentUser()?.firstName }} {{ authService.currentUser()?.lastName }}</strong>
            <span class="text-muted">{{ authService.currentUser()?.email }}</span>
          </div>
          <mat-divider />
          @if (authService.isStudent()) {
            <a mat-menu-item routerLink="/profile">
              <mat-icon>person</mat-icon> My Profile
            </a>
            <a mat-menu-item routerLink="/applications">
              <mat-icon>assignment</mat-icon> My Applications
            </a>
          }
          @if (authService.isEmployer()) {
            <a mat-menu-item routerLink="/employer/dashboard">
              <mat-icon>dashboard</mat-icon> Dashboard
            </a>
          }
          @if (authService.isAdmin()) {
            <a mat-menu-item routerLink="/admin/dashboard">
              <mat-icon>admin_panel_settings</mat-icon> Admin Panel
            </a>
          }
          <mat-divider />
          <button mat-menu-item (click)="authService.logout()">
            <mat-icon>logout</mat-icon> Logout
          </button>
        </mat-menu>
      } @else {
        <a routerLink="/auth/login" mat-button>Login</a>
        <a routerLink="/auth/register" mat-raised-button color="primary">Register</a>
      }
    </mat-toolbar>
  `,
  styleUrl: './header.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent {
  readonly authService = inject(AuthService);
  readonly menuToggled = output<void>();
}
