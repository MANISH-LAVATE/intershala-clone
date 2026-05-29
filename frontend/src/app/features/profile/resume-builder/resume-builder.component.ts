import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-resume-builder',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page-placeholder"><h2>Resume Builder</h2></div>`,
  styles: [`.page-placeholder { padding: 32px; }`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResumeBuilderComponent {}
