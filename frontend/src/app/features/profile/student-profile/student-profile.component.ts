import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page-placeholder"><h2>My Profile</h2><p>Feature coming in Day 3.</p></div>`,
  styles: [`.page-placeholder { padding: 32px; }`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StudentProfileComponent {}
