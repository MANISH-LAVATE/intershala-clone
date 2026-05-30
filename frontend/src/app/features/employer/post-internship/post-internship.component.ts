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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSnackBar } from '@angular/material/snack-bar';
import { InternshipService } from '../../internships/services/internship.service';
import { LookupService } from '../../../core/services/lookup.service';
import { AuthService } from '../../../core/auth/services/auth.service';

function stipendRangeValidator(control: AbstractControl): ValidationErrors | null {
  const parent = control.parent;
  if (!parent) return null;
  const min = parent.get('stipendMin')?.value as number;
  const max = control.value as number;
  if (min && max && max < min) return { stipendRange: true };
  return null;
}

@Component({
  selector: 'app-post-internship',
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
    MatCheckboxModule,
    MatSlideToggleModule,
    MatChipsModule,
    MatAutocompleteModule,
  ],
  template: `
    <div class="post-internship-page">
      <div class="post-internship-page__header">
        <a routerLink="/employer/dashboard" mat-icon-button aria-label="Back to dashboard">
          <mat-icon>arrow_back</mat-icon>
        </a>
        <div>
          <h1>Post an Internship</h1>
          <p class="text-muted">Reach thousands of talented students</p>
        </div>
      </div>

      <form [formGroup]="form" (ngSubmit)="onSubmit()" class="post-internship-page__form" novalidate>
        <!-- Basic Info -->
        <section class="post-internship-page__section">
          <h2>Basic Information</h2>

          <mat-form-field>
            <mat-label>Internship Title</mat-label>
            <input matInput formControlName="title" placeholder="e.g. Software Developer Intern" />
            <mat-error>
              @if (form.get('title')?.hasError('required')) { Title is required }
              @else if (form.get('title')?.hasError('maxlength')) { Max 200 characters }
            </mat-error>
          </mat-form-field>

          <div class="post-internship-page__row">
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
              <mat-label>Internship Type</mat-label>
              <mat-select formControlName="internshipType">
                <mat-option value="InOffice">In Office</mat-option>
                <mat-option value="Remote">Work from Home</mat-option>
                <mat-option value="Hybrid">Hybrid</mat-option>
              </mat-select>
              <mat-error>Type is required</mat-error>
            </mat-form-field>
          </div>

          @if (form.get('internshipType')?.value !== 'Remote') {
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

        <!-- Duration & Openings -->
        <section class="post-internship-page__section">
          <h2>Duration & Openings</h2>

          <div class="post-internship-page__row">
            <mat-form-field>
              <mat-label>Duration (months)</mat-label>
              <mat-select formControlName="durationMonths">
                @for (m of [1, 2, 3, 4, 5, 6]; track m) {
                  <mat-option [value]="m">{{ m }} {{ m === 1 ? 'Month' : 'Months' }}</mat-option>
                }
              </mat-select>
              <mat-error>Duration is required</mat-error>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Number of Openings</mat-label>
              <input matInput type="number" formControlName="openingsCount" min="1" />
              <mat-error>
                @if (form.get('openingsCount')?.hasError('min')) { Min 1 opening }
              </mat-error>
            </mat-form-field>
          </div>

          <div class="post-internship-page__row">
            <mat-form-field>
              <mat-label>Start Date</mat-label>
              <mat-select formControlName="startDateType">
                <mat-option value="Immediate">Immediately</mat-option>
                <mat-option value="Flexible">Flexible</mat-option>
              </mat-select>
            </mat-form-field>

            <mat-form-field>
              <mat-label>Application Deadline</mat-label>
              <input matInput type="date" formControlName="applicationDeadline" />
            </mat-form-field>
          </div>
        </section>

        <!-- Stipend -->
        <section class="post-internship-page__section">
          <h2>Stipend</h2>
          <mat-slide-toggle formControlName="isPaid" class="post-internship-page__toggle">
            This is a paid internship
          </mat-slide-toggle>

          @if (form.get('isPaid')?.value) {
            <div class="post-internship-page__row">
              <mat-form-field>
                <mat-label>Min Stipend (₹/month)</mat-label>
                <input matInput type="number" formControlName="stipendMin" min="0" />
                <mat-hint>Leave blank if not sure</mat-hint>
              </mat-form-field>
              <mat-form-field>
                <mat-label>Max Stipend (₹/month)</mat-label>
                <input matInput type="number" formControlName="stipendMax" min="0" />
                <mat-error>
                  @if (form.get('stipendMax')?.hasError('stipendRange')) { Max must be ≥ Min }
                </mat-error>
              </mat-form-field>
            </div>
          }
        </section>

        <!-- Description -->
        <section class="post-internship-page__section">
          <h2>About the Internship</h2>

          <mat-form-field>
            <mat-label>Description</mat-label>
            <textarea
              matInput
              formControlName="description"
              rows="6"
              placeholder="Describe the internship role, learning opportunities, and what makes it special..."
            ></textarea>
            <mat-hint>{{ form.get('description')?.value?.length ?? 0 }} / 2000</mat-hint>
            <mat-error>
              @if (form.get('description')?.hasError('required')) { Description is required }
              @else if (form.get('description')?.hasError('minlength')) { Minimum 50 characters }
            </mat-error>
          </mat-form-field>

          <mat-form-field>
            <mat-label>Responsibilities (optional)</mat-label>
            <textarea
              matInput
              formControlName="responsibilities"
              rows="4"
              placeholder="List the key responsibilities..."
            ></textarea>
          </mat-form-field>

          <mat-form-field>
            <mat-label>Requirements (optional)</mat-label>
            <textarea
              matInput
              formControlName="requirements"
              rows="4"
              placeholder="List any specific requirements or preferred skills..."
            ></textarea>
          </mat-form-field>
        </section>

        @if (errorMessage()) {
          <div class="post-internship-page__error" role="alert">
            <mat-icon>error_outline</mat-icon>
            {{ errorMessage() }}
          </div>
        }

        <div class="post-internship-page__actions">
          <a mat-stroked-button routerLink="/employer/dashboard">Cancel</a>
          <button
            mat-raised-button
            color="primary"
            type="submit"
            class="post-internship-page__submit"
            [disabled]="isSubmitting()"
          >
            @if (isSubmitting()) { <mat-spinner diameter="20" /> Posting... }
            @else { Post Internship }
          </button>
        </div>
      </form>
    </div>
  `,
  styleUrl: './post-internship.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostInternshipComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(InternshipService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);
  readonly lookupService = inject(LookupService);

  readonly isSubmitting = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    categoryId: [0, [Validators.required, Validators.min(1)]],
    internshipType: ['InOffice', [Validators.required]],
    locationId: [null as number | null],
    durationMonths: [3, [Validators.required, Validators.min(1), Validators.max(24)]],
    openingsCount: [1, [Validators.required, Validators.min(1)]],
    startDateType: ['Immediate'],
    applicationDeadline: [null as string | null],
    isPaid: [true],
    stipendMin: [null as number | null],
    stipendMax: [null as number | null, [stipendRangeValidator]],
    description: ['', [Validators.required, Validators.minLength(50), Validators.maxLength(2000)]],
    responsibilities: [''],
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

    const user = this.authService.currentUser();
    if (!user) return;

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const val = this.form.getRawValue();

    this.service
      .createInternship({
        title: val.title,
        description: val.description,
        responsibilities: val.responsibilities || null,
        requirements: val.requirements || null,
        categoryId: val.categoryId,
        locationId: val.locationId,
        internshipType: val.internshipType as 'InOffice' | 'Remote' | 'Hybrid',
        stipendMin: val.stipendMin,
        stipendMax: val.stipendMax,
        isPaid: val.isPaid,
        durationMonths: val.durationMonths,
        startDateType: val.startDateType,
        startDate: null,
        openingsCount: val.openingsCount,
        applicationDeadline: val.applicationDeadline || null,
        skillIds: val.skillIds,
      })
      .subscribe({
        next: (res) => {
          this.isSubmitting.set(false);
          if (res.success) {
            this.snackBar.open('Internship posted successfully!', 'View', { duration: 5000 });
            this.router.navigate(['/internships', res.data]);
          }
        },
        error: () => {
          this.isSubmitting.set(false);
          this.errorMessage.set('Failed to post internship. Please try again.');
        },
      });
  }
}
