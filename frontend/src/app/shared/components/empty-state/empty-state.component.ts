import { Component, input, output, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  template: `
    <div class="empty-state" role="status" [attr.aria-label]="title()">
      <mat-icon class="empty-state__icon">{{ icon() }}</mat-icon>
      <h3 class="empty-state__title">{{ title() }}</h3>
      <p class="empty-state__subtitle">{{ subtitle() }}</p>
      @if (actionLabel()) {
        <button mat-stroked-button color="primary" (click)="actionClicked.emit()">
          {{ actionLabel() }}
        </button>
      }
    </div>
  `,
  styleUrl: './empty-state.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmptyStateComponent {
  readonly icon = input('search_off');
  readonly title = input('No results found');
  readonly subtitle = input('Try adjusting your filters or search terms.');
  readonly actionLabel = input<string | null>(null);
  readonly actionClicked = output<void>();
}
