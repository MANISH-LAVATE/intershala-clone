import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { debounceTime, distinctUntilChanged, catchError, EMPTY } from 'rxjs';
import { AdminService, AdminUser } from '../services/admin.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { PaginationMeta } from '../../../shared/models/api-response.model';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatTableModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatSlideToggleModule,
    MatPaginatorModule,
    SkeletonLoaderComponent,
    EmptyStateComponent,
  ],
  template: `
    <div class="user-management-page">
      <div class="user-management-page__header">
        <a mat-button routerLink="/admin/dashboard">
          <mat-icon>arrow_back</mat-icon>
          Dashboard
        </a>
        <h1 class="user-management-page__title">User Management</h1>
      </div>

      <!-- Filters -->
      <div class="user-management-page__filters">
        <mat-form-field appearance="outline" class="filter-search">
          <mat-icon matPrefix>search</mat-icon>
          <input matInput [formControl]="searchControl" placeholder="Search by name or email…" />
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Role</mat-label>
          <mat-select [formControl]="roleFilter" (selectionChange)="onFilterChange()">
            <mat-option value="">All Roles</mat-option>
            <mat-option value="Student">Students</mat-option>
            <mat-option value="Employer">Employers</mat-option>
            <mat-option value="Admin">Admins</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Status</mat-label>
          <mat-select [formControl]="statusFilter" (selectionChange)="onFilterChange()">
            <mat-option value="">All</mat-option>
            <mat-option value="true">Active</mat-option>
            <mat-option value="false">Inactive</mat-option>
          </mat-select>
        </mat-form-field>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="8" />
      } @else if (users().length === 0) {
        <app-empty-state
          icon="people"
          title="No users found"
          subtitle="Try adjusting the search filters."
        />
      } @else {
        <mat-card>
          <mat-card-content>
            <table mat-table [dataSource]="users()" class="users-table" aria-label="User list">
              <ng-container matColumnDef="name">
                <th mat-header-cell *matHeaderCellDef>Name</th>
                <td mat-cell *matCellDef="let user">
                  <div class="user-name">
                    <div class="user-avatar" aria-hidden="true">{{ user.firstName[0] }}</div>
                    <div>
                      <strong>{{ user.firstName }} {{ user.lastName }}</strong>
                      <br />
                      <span class="text-muted">{{ user.email }}</span>
                    </div>
                  </div>
                </td>
              </ng-container>

              <ng-container matColumnDef="role">
                <th mat-header-cell *matHeaderCellDef>Role</th>
                <td mat-cell *matCellDef="let user">
                  <mat-chip class="role-chip role-chip--{{ user.role.toLowerCase() }}">
                    {{ user.role }}
                  </mat-chip>
                </td>
              </ng-container>

              <ng-container matColumnDef="verified">
                <th mat-header-cell *matHeaderCellDef>Verified</th>
                <td mat-cell *matCellDef="let user">
                  @if (user.isEmailVerified) {
                    <mat-icon color="primary" aria-label="Verified">check_circle</mat-icon>
                  } @else {
                    <mat-icon aria-label="Not verified" style="color:#bbb">radio_button_unchecked</mat-icon>
                  }
                </td>
              </ng-container>

              <ng-container matColumnDef="joined">
                <th mat-header-cell *matHeaderCellDef>Joined</th>
                <td mat-cell *matCellDef="let user">
                  {{ user.createdAt | date:'mediumDate' }}
                </td>
              </ng-container>

              <ng-container matColumnDef="status">
                <th mat-header-cell *matHeaderCellDef>Active</th>
                <td mat-cell *matCellDef="let user">
                  <mat-slide-toggle
                    [checked]="user.isActive"
                    (change)="toggleStatus(user, $event.checked)"
                    [attr.aria-label]="'Toggle active status for ' + user.firstName"
                  />
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="columns"></tr>
              <tr mat-row *matRowDef="let row; columns: columns;"></tr>
            </table>
          </mat-card-content>
        </mat-card>

        @if (pagination() && pagination()!.totalPages > 1) {
          <mat-paginator
            [length]="pagination()!.totalCount"
            [pageSize]="page.pageSize"
            [pageIndex]="page.page - 1"
            [pageSizeOptions]="[10, 20, 50]"
            (page)="onPageChange($event)"
            aria-label="User list pagination"
          />
        }
      }
    </div>
  `,
  styleUrl: './user-management.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserManagementComponent implements OnInit {
  private readonly service = inject(AdminService);
  private readonly snackBar = inject(MatSnackBar);

  readonly columns = ['name', 'role', 'verified', 'joined', 'status'];
  readonly loading = signal(false);
  readonly users = signal<AdminUser[]>([]);
  readonly pagination = signal<PaginationMeta | null>(null);

  page = { page: 1, pageSize: 20 };

  readonly searchControl = new FormControl('');
  readonly roleFilter = new FormControl('');
  readonly statusFilter = new FormControl('');

  ngOnInit(): void {
    this.searchControl.valueChanges
      .pipe(debounceTime(350), distinctUntilChanged())
      .subscribe(() => {
        this.page.page = 1;
        this.load();
      });

    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.service
      .getUsers({
        search: this.searchControl.value || '',
        role: this.roleFilter.value || '',
        isActive: this.statusFilter.value || '',
        page: this.page.page,
        pageSize: this.page.pageSize,
      })
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to load users.', 'Dismiss', { duration: 4000 });
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.users.set(res.data ?? []);
        this.pagination.set(res.pagination);
        this.loading.set(false);
      });
  }

  onFilterChange(): void {
    this.page.page = 1;
    this.load();
  }

  onPageChange(event: PageEvent): void {
    this.page = { page: event.pageIndex + 1, pageSize: event.pageSize };
    this.load();
  }

  toggleStatus(user: AdminUser, isActive: boolean): void {
    this.service
      .updateUserStatus(user.id, isActive)
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to update user.', 'Dismiss', { duration: 4000 });
        return EMPTY;
      }))
      .subscribe(() => {
        this.snackBar.open(`User ${isActive ? 'activated' : 'deactivated'}.`, undefined, { duration: 2000 });
        this.users.update((list) =>
          list.map((u) => u.id === user.id ? { ...u, isActive } : u),
        );
      });
  }
}
