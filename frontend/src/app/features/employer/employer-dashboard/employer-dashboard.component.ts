import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employer-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page-placeholder"><h2>Employer Dashboard</h2><p>Feature coming in Day 2.</p></div>`,
  styles: [`.page-placeholder { padding: 32px; }`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EmployerDashboardComponent {}
