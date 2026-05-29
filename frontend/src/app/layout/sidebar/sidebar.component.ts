import { Component, ChangeDetectionStrategy, inject, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../core/auth/services/auth.service';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  roles?: string[];
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, MatListModule, MatIconModule, MatDividerModule],
  template: `
    <nav class="sidebar" aria-label="Side navigation">
      <mat-nav-list>
        @for (item of visibleNavItems(); track item.route) {
          <a
            mat-list-item
            [routerLink]="item.route"
            routerLinkActive="active"
            (click)="linkClicked.emit()"
            [attr.aria-label]="item.label"
          >
            <mat-icon matListItemIcon>{{ item.icon }}</mat-icon>
            <span matListItemTitle>{{ item.label }}</span>
          </a>
        }
      </mat-nav-list>
    </nav>
  `,
  styleUrl: './sidebar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
  readonly authService = inject(AuthService);
  readonly linkClicked = output<void>();

  private readonly allNavItems: NavItem[] = [
    { label: 'Internships', icon: 'work_outline', route: '/internships' },
    { label: 'Jobs', icon: 'business_center', route: '/jobs' },
    { label: 'Courses', icon: 'school', route: '/courses' },
    { label: 'My Applications', icon: 'assignment', route: '/applications', roles: ['Student'] },
    { label: 'My Profile', icon: 'person', route: '/profile', roles: ['Student'] },
    { label: 'Dashboard', icon: 'dashboard', route: '/employer/dashboard', roles: ['Employer'] },
    { label: 'Post Internship', icon: 'add_box', route: '/employer/post-internship', roles: ['Employer'] },
    { label: 'Post Job', icon: 'add_business', route: '/employer/post-job', roles: ['Employer'] },
    { label: 'Applications', icon: 'people', route: '/employer/applications', roles: ['Employer'] },
    { label: 'Admin Panel', icon: 'admin_panel_settings', route: '/admin/dashboard', roles: ['Admin'] },
  ];

  visibleNavItems(): NavItem[] {
    const role = this.authService.currentUser()?.role;
    return this.allNavItems.filter(
      (item) => !item.roles || (role && item.roles.includes(role)),
    );
  }
}
