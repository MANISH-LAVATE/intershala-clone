import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { HeaderComponent } from '../header/header.component';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { FooterComponent } from '../footer/footer.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatSidenavModule,
    HeaderComponent,
    SidebarComponent,
    FooterComponent,
  ],
  template: `
    <div class="main-layout">
      <app-header (menuToggled)="toggleSidebar()" />
      <mat-sidenav-container class="main-layout__container">
        <mat-sidenav
          [mode]="isMobile() ? 'over' : 'side'"
          [opened]="!isMobile() || sidebarOpen()"
          class="main-layout__sidenav"
        >
          <app-sidebar (linkClicked)="onLinkClicked()" />
        </mat-sidenav>
        <mat-sidenav-content class="main-layout__content">
          <main>
            <router-outlet />
          </main>
          <app-footer />
        </mat-sidenav-content>
      </mat-sidenav-container>
    </div>
  `,
  styleUrl: './main-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainLayoutComponent {
  private readonly breakpointObserver = inject(BreakpointObserver);

  readonly isMobile = signal(false);
  readonly sidebarOpen = signal(false);

  constructor() {
    this.breakpointObserver.observe([Breakpoints.Handset, Breakpoints.TabletPortrait]).subscribe((result) => {
      this.isMobile.set(result.matches);
      if (result.matches) {
        this.sidebarOpen.set(false);
      }
    });
  }

  toggleSidebar(): void {
    this.sidebarOpen.update((v) => !v);
  }

  onLinkClicked(): void {
    if (this.isMobile()) {
      this.sidebarOpen.set(false);
    }
  }
}
