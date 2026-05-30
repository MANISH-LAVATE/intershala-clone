import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import {
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { JobService } from '../../jobs/services/job.service';
import { LookupService } from '../../../core/services/lookup.service';

function salaryRangeValidator(control: AbstractControl): ValidationErrors | null {
  const parent = control.parent;
  if (!parent) return null;
  const min = parent.get('salaryMin')?.value as number;
  const max = control.value as number;
  if (min && max && max < min) return { salaryRange: true };
  return null;
}

@Component({
  selector: 'app-post-job',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
  ],
  template: `
    <div class="post-job-page">
      <div class="post-job-page__header">
        <a routerLink="/employer/dashboard" mat-icon-button aria-label="Back to dashboard">
          <mat-icon>arrow_back</mat-icon>
        </a>
        <div>
          <h1>Post a Job</h1>
          <p class="text-muted">Hire the right full-time talent</p>
        </div>
      </div>

      <form [formGroup]="form" (ngSubmit)="onSubmit()" class="post-job-page__form" novalidate>
        <section class="post-job-page__section">
          <h2>Basic Information</h2>

          <mat-form-field>
            <mat-label>Job Title</mat-label>
            <input matInput formControlName="title" placeholder="e.g. Full Stack Developer" />
            <mat-error>
              @if (form.get('title')?.hasError('required')) { Title is required }
              @else if (form.get('title')?.hasError('maxlength')) { Max 200 characters }
            </mat-error>
          </mat-form-field>

          <div class="post-job-page__row">
            <mat-form-field>
              <mat-label>Category</mat-label>
              <mat-select formControlName="categoryId">
                @for (cat of lookupService.categories(); track cat.id) {
                  <mat-option [value]="cat.id">{{ cat.name }}</mat-option>
                }
              </mat-select>
              <mat-error>Category is required</mat-error>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Job Type</mat-label>
              <mat-select formControlName="jobType">
                <mat-option value="InOffice">In Office</mat-option>
                <mat-option value="Remote">Remote</mat-option>
                <mat-option value="Hybrid">Hybrid</mat-option>
              </mat-select>
              <mat-error>Type is required</mat-error>
            </mat-form-field>
          </div>

          @if (form.get('jobType')?.value !== 'Remote') {
            <mat-form-field>
              <mat-label>Location</mat-label>
              <mat-select formControlName="locationId">
                <mat-option [value]="null">Select City</mat-option>
                @for (loc of lookupService.locations(); track loc.id) {
                  <mat-option [value]="loc.id">{{ loc.cityName }}, {{ loc.state }}</mat-option>
                }
              </mat-select>
            </mat-form-field>
          }
        </section>

        <section class="post-job-page__section">
          <h2>Compensation & Experience</h2>

          <div class="post-job-page__row">
            <mat-form-field>
              <mat-label>Min Salary (₹/year)</mat-label>
              <input matInput type="number" formControlName="salaryMin" min="0" />
            </mat-form-field>
            <mat-form-field>
              <mat-label>Max Salary (₹/year)</mat-label>
              <input matInput type="number" formControlName="salaryMax" min="0" />
              <mat-error>
                @if (form.get('salaryMax')?.hasError('salaryRange')) { Max must be ≥ Min }
              </mat-error>
            </mat-form-field>
          </div>

          <div class="post-job-page__row">
            <mat-form-field>
              <mat-label>Min Experience (years)</mat-label>
              <input matInput type="number" formControlName="experienceYearsMin" min="0" />
            </mat-form-field>
            <mat-form-field>
              <mat-label>Application Deadline</mat-label>
              <input matInput type="date" formControlName="applicationDeadline" />
            </mat-form-field>
          </div>
        </section>

        <section class="post-job-page__section">
          <h2>About the Role</h2>

          <mat-form-field>
            <mat-label>Description</mat-label>
            <textarea
              matInput
              formControlName="description"
              rows="6"
              placeholder="Describe the role, team, and responsibilities..."
            ></textarea>
            <mat-hint>{{ form.get('description')?.value?.length ?? 0 }} / 2000</mat-hint>
            <mat-error>
              @if (form.get('description')?.hasError('required')) { Description is required }
              @else if (form.get('description')?.hasError('minlength')) { Minimum 50 characters }
            </mat-error>
          </mat-form-field>

          <mat-form-field>
            <mat-label>Requirements (optional)</mat-label>
            <textarea
              matInput
              formControlName="requirements"
              rows="4"
              placeholder="List required qualifications and skills..."
            ></textarea>
          </mat-form-field>
        </section>

        @if (errorMessage()) {
          <div class="post-job-page__error" role="alert">
            <mat-icon>error_outline</mat-icon>
            {{ errorMessage() }}
          </div>
        }

        <div class="post-job-page__actions">
          <a mat-stroked-button routerLink="/employer/dashboard">Cancel</a>
          <button
            mat-raised-button
            color="primary"
            type="submit"
            class="post-job-page__submit"
            [disabled]="isSubmitting()"
          >
            @if (isSubmitting()) { <mat-spinner diameter="20" /> Posting... }
            @else { Post Job }
          </button>
        </div>
      </form>
    </div>
  `,
  styleUrl: './post-job.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostJobComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(JobService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);
  readonly lookupService = inject(LookupService);

  readonly isSubmitting = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    jobType: ['InOffice', [Validators.required]],
    locationId: [null as number | null],
    salaryMin: [null as number | null],
    salaryMax: [null as number | null, [salaryRangeValidator]],
    experienceYearsMin: [null as number | null],
    applicationDeadline: [null as string | null],
    description: ['', [Validators.required, Validators.minLength(50), Validators.maxLength(2000)]],
    requirements: [''],
    skillIds: [[] as number[]],
  });

  ngOnInit(): void {
    this.lookupService.loadCategories();
    this.lookupService.loadLocations();
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const val = this.form.getRawValue();

    this.service
      .createJob({
        title: val.title,
        description: val.description,
        requirements: val.requirements || null,
        categoryId: val.categoryId,
        locationId: val.locationId,
        jobType: val.jobType as 'InOffice' | 'Remote' | 'Hybrid',
        salaryMin: val.salaryMin,
        salaryMax: val.salaryMax,
        experienceYearsMin: val.experienceYearsMin,
        applicationDeadline: val.applicationDeadline || null,
        skillIds: val.skillIds,
      })
      .subscribe({
        next: (res) => {
          this.isSubmitting.set(false);
          if (res.success) {
            this.snackBar.open('Job posted successfully!', 'View', { duration: 5000 });
            this.router.navigate(['/jobs', res.data]);
          }
        },
        error: () => {
          this.isSubmitting.set(false);
          this.errorMessage.set('Failed to post job. Please try again.');
        },
      });
  }
}
