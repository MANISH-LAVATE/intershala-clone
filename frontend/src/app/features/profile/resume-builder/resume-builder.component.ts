import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, EMPTY } from 'rxjs';
import { ProfileService } from '../services/profile.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import { StudentProfile } from '../models/profile.model';

@Component({
  selector: 'app-resume-builder',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    SkeletonLoaderComponent,
  ],
  template: `
    <div class="resume-page">
      <div class="resume-page__header">
        <a mat-button routerLink="/profile">
          <mat-icon>arrow_back</mat-icon>
          My Profile
        </a>
        <h1 class="resume-page__title">Resume Preview</h1>
        <button mat-raised-button color="primary" (click)="printResume()" aria-label="Print resume">
          <mat-icon>print</mat-icon>
          Print / Download PDF
        </button>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="5" />
      } @else if (profile()) {
        <mat-card class="resume-card" id="resume-content">
          <mat-card-content>
            <section class="resume-section resume-header">
              <h2 class="resume-name">{{ profile()!.firstName }} {{ profile()!.lastName }}</h2>
              <div class="resume-contact">
                <span>
                  <mat-icon aria-hidden="true">email</mat-icon>
                  {{ profile()!.email }}
                </span>
                @if (profile()!.phoneNumber) {
                  <span>
                    <mat-icon aria-hidden="true">phone</mat-icon>
                    {{ profile()!.phoneNumber }}
                  </span>
                }
                @if (profile()!.linkedInUrl) {
                  <span>
                    <mat-icon aria-hidden="true">link</mat-icon>
                    {{ profile()!.linkedInUrl }}
                  </span>
                }
                @if (profile()!.gitHubUrl) {
                  <span>
                    <mat-icon aria-hidden="true">code</mat-icon>
                    {{ profile()!.gitHubUrl }}
                  </span>
                }
                @if (profile()!.portfolioUrl) {
                  <span>
                    <mat-icon aria-hidden="true">web</mat-icon>
                    {{ profile()!.portfolioUrl }}
                  </span>
                }
              </div>
            </section>

            @if (profile()!.bio) {
              <mat-divider />
              <section class="resume-section">
                <h3 class="resume-section__title">Summary</h3>
                <p class="resume-bio">{{ profile()!.bio }}</p>
              </section>
            }

            @if (profile()!.educations.length > 0) {
              <mat-divider />
              <section class="resume-section">
                <h3 class="resume-section__title">Education</h3>
                <ul class="resume-list" aria-label="Education history">
                  @for (edu of profile()!.educations; track edu.id) {
                    <li class="resume-list__item">
                      <div class="resume-list__header">
                        <strong>{{ edu.institution }}</strong>
                        <span class="resume-list__period">
                          {{ edu.startYear }} — {{ edu.isCurrent ? 'Present' : edu.endYear }}
                        </span>
                      </div>
                      <p class="resume-list__subtitle">
                        {{ edu.degree }}, {{ edu.fieldOfStudy }}
                        @if (edu.grade) { · {{ edu.grade }} }
                      </p>
                    </li>
                  }
                </ul>
              </section>
            }

            @if (profile()!.experiences.length > 0) {
              <mat-divider />
              <section class="resume-section">
                <h3 class="resume-section__title">Work Experience</h3>
                <ul class="resume-list" aria-label="Work experience">
                  @for (exp of profile()!.experiences; track exp.id) {
                    <li class="resume-list__item">
                      <div class="resume-list__header">
                        <strong>{{ exp.title }}</strong>
                        <span class="resume-list__period">
                          {{ exp.startDate | date:'MMM yyyy' }} —
                          {{ exp.isCurrent ? 'Present' : (exp.endDate | date:'MMM yyyy') }}
                        </span>
                      </div>
                      <p class="resume-list__subtitle">{{ exp.company }}</p>
                      @if (exp.description) {
                        <p class="resume-list__description">{{ exp.description }}</p>
                      }
                    </li>
                  }
                </ul>
              </section>
            }

            @if (!profile()!.bio && profile()!.educations.length === 0 && profile()!.experiences.length === 0) {
              <div class="resume-empty">
                <mat-icon aria-hidden="true">info_outline</mat-icon>
                <p>Complete your profile to populate the resume preview.</p>
                <a mat-stroked-button routerLink="/profile">Complete Profile</a>
              </div>
            }
          </mat-card-content>
        </mat-card>

        <p class="resume-tip">
          <mat-icon aria-hidden="true">tips_and_updates</mat-icon>
          Use your browser's print function (Ctrl+P / Cmd+P) and select "Save as PDF" for best results.
        </p>
      }
    </div>
  `,
  styleUrl: './resume-builder.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResumeBuilderComponent implements OnInit {
  private readonly service = inject(ProfileService);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly profile = signal<StudentProfile | null>(null);

  ngOnInit(): void {
    this.loading.set(true);
    this.service
      .getProfile()
      .pipe(catchError(() => {
        this.snackBar.open('Failed to load profile.', 'Dismiss', { duration: 4000 });
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.profile.set(res.data);
        this.loading.set(false);
      });
  }

  printResume(): void {
    window.print();
  }
}
