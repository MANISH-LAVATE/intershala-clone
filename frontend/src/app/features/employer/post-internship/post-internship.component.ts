import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-post-internship',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page-placeholder"><h2>Post Internship</h2></div>`,
  styles: [`.page-placeholder { padding: 32px; }`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostInternshipComponent {}
