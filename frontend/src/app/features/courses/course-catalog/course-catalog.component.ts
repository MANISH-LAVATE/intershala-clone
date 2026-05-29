import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-course-catalog',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page-placeholder"><h2>Courses</h2><p>Feature coming in Day 4.</p></div>`,
  styles: [`.page-placeholder { padding: 32px; }`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CourseCatalogComponent {}
