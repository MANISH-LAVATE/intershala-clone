import {
  Component,
  ChangeDetectionStrategy,
  inject,
  output,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { AuthService } from '../../core/auth/services/auth.service';
import { NotificationService } from '../../core/notifications/notification.service';
import { TokenStorageService } from '../../core/auth/services/token-storage.service';
import { NotificationItem } from '../../core/notifications/notification.model';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    ReactiveFormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatBadgeModule,
    MatDividerModule,
    MatFormFieldModule,
    MatInputModule,
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

      <!-- Search bar -->
      <div class="header__search">
        <mat-form-field appearance="outline" class="header__search-field">
          <mat-icon matPrefix>search</mat-icon>
          <input
            matInput
            [formControl]="searchControl"
            placeholder="Search internships, jobs, courses…"
            aria-label="Global search"
          />
        </mat-form-field>
      </div>

      <span class="header__spacer"></span>

      @if (authService.isAuthenticated()) {
        <!-- Notification bell -->
        <button
          mat-icon-button
          aria-label="Notifications"
          [matMenuTriggerFor]="notificationMenu"
          (menuOpened)="onNotificationsOpen()"
          [matBadge]="notificationService.unreadCount() > 0 ? notificationService.unreadCount().toString() : null"
          matBadgeColor="warn"
        >
          <mat-icon>notifications_none</mat-icon>
        </button>

        <mat-menu #notificationMenu="matMenu" class="notification-menu">
          <div class="notification-panel">
            <div class="notification-panel__header">
              <strong>Notifications</strong>
              @if (notificationService.unreadCount() > 0) {
                <button mat-button color="primary" (click)="markAllRead()">
                  Mark all read
                </button>
              }
            </div>
            @if (notificationService.notifications().length === 0) {
              <div class="notification-panel__empty">
                <mat-icon>notifications_none</mat-icon>
                <p>No notifications</p>
              </div>
            } @else {
              @for (n of notificationService.notifications().slice(0, 8); track n.id) {
                <div
                  class="notification-item"
                  [class.notification-item--unread]="!n.isRead"
                  (click)="onNotificationClick(n)"
                  (keydown)="$event.key === 'Enter' && onNotificationClick(n)"
                  role="button"
                  [attr.aria-label]="n.title"
                  tabindex="0"
                >
                  <div class="notification-item__dot" [class.visible]="!n.isRead"></div>
                  <div class="notification-item__content">
                    <p class="notification-item__title">{{ n.title }}</p>
                    <p class="notification-item__message">{{ n.message }}</p>
                    <p class="notification-item__time">{{ n.createdAt | date:'shortTime' }}</p>
                  </div>
                </div>
              }
            }
            <mat-divider />
            <a mat-button routerLink="/notifications" class="notification-panel__see-all">
              See all notifications
            </a>
          </div>
        </mat-menu>

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
            <a mat-menu-item routerLink="/courses/my">
              <mat-icon>school</mat-icon> My Courses
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
          <button mat-menu-item (click)="logout()">
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
export class HeaderComponent implements OnInit, OnDestroy {
  readonly authService = inject(AuthService);
  readonly notificationService = inject(NotificationService);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);
  readonly menuToggled = output<void>();

  readonly searchControl = new FormControl('');

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.notificationService.loadNotifications();
      const token = this.tokenStorage.getAccessToken();
      if (token) {
        this.notificationService.startSignalRConnection(token);
      }
    }

    this.searchControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((q) => {
        if (q && q.trim().length >= 2) {
          this.router.navigate(['/search'], { queryParams: { q } });
        }
      });
  }

  ngOnDestroy(): void {
    this.notificationService.stopSignalRConnection();
  }

  onNotificationsOpen(): void {
    this.notificationService.loadNotifications();
  }

  onNotificationClick(n: NotificationItem): void {
    if (!n.isRead) {
      this.notificationService.markRead(n.id).subscribe();
      this.notificationService.unreadCount.update((c) => Math.max(0, c - 1));
    }
    if (n.actionUrl) {
      this.router.navigateByUrl(n.actionUrl);
    }
  }

  markAllRead(): void {
    this.notificationService.markAllRead().subscribe(() => {
      this.notificationService.unreadCount.set(0);
    });
  }

  logout(): void {
    this.notificationService.stopSignalRConnection();
    this.authService.logout();
  }
}
