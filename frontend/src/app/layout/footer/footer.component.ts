import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <footer class="footer" role="contentinfo">
      <div class="container footer__content">
        <div class="footer__brand">
          <span class="footer__logo">Internshala Clone</span>
          <p>Helping students achieve their dream career.</p>
        </div>
        <div class="footer__links">
          <div class="footer__column">
            <h4>Students</h4>
            <a routerLink="/internships">Internships</a>
            <a routerLink="/jobs">Jobs</a>
            <a routerLink="/courses">Courses</a>
          </div>
          <div class="footer__column">
            <h4>Employers</h4>
            <a routerLink="/auth/register">Post Internship</a>
            <a routerLink="/auth/register">Post Job</a>
          </div>
          <div class="footer__column">
            <h4>Company</h4>
            <a href="#">About Us</a>
            <a href="#">Blog</a>
            <a href="#">Contact</a>
          </div>
        </div>
      </div>
      <div class="footer__bottom">
        <div class="container">
          <span>&copy; {{ year }} Internshala Clone. All rights reserved.</span>
        </div>
      </div>
    </footer>
  `,
  styleUrl: './footer.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FooterComponent {
  readonly year = new Date().getFullYear();
}
