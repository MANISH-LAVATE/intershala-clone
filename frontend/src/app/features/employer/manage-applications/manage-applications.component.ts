import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manage-applications',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="page-placeholder"><h2>Manage Applications</h2></div>`,
  styles: [`.page-placeholder { padding: 32px; }`],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ManageApplicationsComponent {}
