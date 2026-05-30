import { Component, input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-skeleton-loader',
  standalone: true,
  imports: [CommonModule],
  template: `
    @for (item of items(); track $index) {
      <div class="skeleton-card" aria-hidden="true">
        <div class="skeleton-card__header">
          <div class="skeleton-circle"></div>
          <div class="skeleton-lines">
            <div class="skeleton-line" style="width:55%"></div>
            <div class="skeleton-line" style="width:35%"></div>
          </div>
        </div>
        <div class="skeleton-line" style="width:70%;margin-top:12px"></div>
        <div class="skeleton-line" style="width:90%;margin-top:8px"></div>
        <div class="skeleton-line" style="width:40%;margin-top:8px"></div>
      </div>
    }
  `,
  styleUrl: './skeleton-loader.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SkeletonLoaderComponent {
  readonly count = input(3);
  items = () => Array.from({ length: this.count() });
}
