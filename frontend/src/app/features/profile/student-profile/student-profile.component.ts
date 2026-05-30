import {
  Component,
  ChangeDetectionStrategy,
  inject,
  signal,
  OnInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { catchError, EMPTY, Observable } from 'rxjs';
import { ProfileService } from '../services/profile.service';
import { SkeletonLoaderComponent } from '../../../shared/components/loader/skeleton-loader.component';
import {
  Education,
  Experience,
  StudentProfile,
  AddEducationForm,
  AddExperienceForm,
} from '../models/profile.model';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatDialogModule,
    MatExpansionModule,
    MatDividerModule,
    SkeletonLoaderComponent,
  ],
  template: `
    <div class="profile-page">
      <div class="profile-page__header">
        <h1 class="profile-page__title">My Profile</h1>
        <a mat-stroked-button routerLink="/profile/resume">
          <mat-icon>description</mat-icon>
          Resume Builder
        </a>
      </div>

      @if (loading()) {
        <app-skeleton-loader [count]="4" />
      } @else if (profile()) {
        <div class="profile-page__content">

          <!-- Completeness banner -->
          <mat-card class="completeness-card">
            <mat-card-content>
              <div class="completeness-card__header">
                <div>
                  <h2 class="completeness-card__name">
                    {{ profile()!.firstName }} {{ profile()!.lastName }}
                  </h2>
                  <p class="completeness-card__email">{{ profile()!.email }}</p>
                </div>
                <div class="completeness-card__score">
                  <span class="completeness-card__percent">{{ profile()!.profileCompleteness }}%</span>
                  <span class="completeness-card__label">Profile Complete</span>
                </div>
              </div>
              <mat-progress-bar
                mode="determinate"
                [value]="profile()!.profileCompleteness"
                [color]="profile()!.profileCompleteness >= 80 ? 'primary' : 'accent'"
                aria-label="Profile completeness"
              />
            </mat-card-content>
          </mat-card>

          <!-- Personal Info -->
          <mat-card>
            <mat-card-header>
              <mat-card-title>Personal Information</mat-card-title>
              <mat-card-subtitle>Basic details and contact info</mat-card-subtitle>
            </mat-card-header>
            <mat-card-content>
              @if (editingProfile()) {
                <form [formGroup]="profileForm" (ngSubmit)="saveProfile()" class="profile-form">
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>First Name</mat-label>
                      <input matInput formControlName="firstName" />
                      @if (profileForm.get('firstName')?.hasError('required')) {
                        <mat-error>First name is required.</mat-error>
                      }
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>Last Name</mat-label>
                      <input matInput formControlName="lastName" />
                      @if (profileForm.get('lastName')?.hasError('required')) {
                        <mat-error>Last name is required.</mat-error>
                      }
                    </mat-form-field>
                  </div>
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>Phone Number</mat-label>
                      <input matInput formControlName="phoneNumber" />
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>Gender</mat-label>
                      <mat-select formControlName="gender">
                        <mat-option value="">Not specified</mat-option>
                        <mat-option value="Male">Male</mat-option>
                        <mat-option value="Female">Female</mat-option>
                        <mat-option value="Non-binary">Non-binary</mat-option>
                        <mat-option value="Prefer not to say">Prefer not to say</mat-option>
                      </mat-select>
                    </mat-form-field>
                  </div>
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>Institution</mat-label>
                      <input matInput formControlName="currentInstitution" />
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>Course / Degree</mat-label>
                      <input matInput formControlName="courseOfStudy" />
                    </mat-form-field>
                  </div>
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>Graduation Year</mat-label>
                      <input matInput type="number" formControlName="graduationYear" />
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>GPA / Percentage</mat-label>
                      <input matInput type="number" step="0.01" formControlName="gpa" />
                    </mat-form-field>
                  </div>
                  <mat-form-field appearance="outline" class="profile-form__full">
                    <mat-label>Bio</mat-label>
                    <textarea matInput formControlName="bio" rows="3" maxlength="1000"></textarea>
                    <mat-hint align="end">{{ profileForm.get('bio')?.value?.length ?? 0 }}/1000</mat-hint>
                  </mat-form-field>
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>LinkedIn URL</mat-label>
                      <input matInput formControlName="linkedInUrl" />
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>GitHub URL</mat-label>
                      <input matInput formControlName="gitHubUrl" />
                    </mat-form-field>
                  </div>
                  <mat-form-field appearance="outline" class="profile-form__full">
                    <mat-label>Portfolio URL</mat-label>
                    <input matInput formControlName="portfolioUrl" />
                  </mat-form-field>
                  <div class="profile-form__actions">
                    <button mat-button type="button" (click)="cancelEdit()">Cancel</button>
                    <button
                      mat-raised-button
                      color="primary"
                      type="submit"
                      [disabled]="savingProfile() || profileForm.invalid"
                    >
                      Save Changes
                    </button>
                  </div>
                </form>
              } @else {
                <div class="profile-details">
                  <div class="profile-details__grid">
                    @if (profile()!.phoneNumber) {
                      <div class="profile-details__item">
                        <mat-icon aria-hidden="true">phone</mat-icon>
                        <div>
                          <span class="profile-details__label">Phone</span>
                          <span>{{ profile()!.phoneNumber }}</span>
                        </div>
                      </div>
                    }
                    @if (profile()!.gender) {
                      <div class="profile-details__item">
                        <mat-icon aria-hidden="true">person</mat-icon>
                        <div>
                          <span class="profile-details__label">Gender</span>
                          <span>{{ profile()!.gender }}</span>
                        </div>
                      </div>
                    }
                    @if (profile()!.currentInstitution) {
                      <div class="profile-details__item">
                        <mat-icon aria-hidden="true">school</mat-icon>
                        <div>
                          <span class="profile-details__label">Institution</span>
                          <span>{{ profile()!.currentInstitution }}</span>
                        </div>
                      </div>
                    }
                    @if (profile()!.courseOfStudy) {
                      <div class="profile-details__item">
                        <mat-icon aria-hidden="true">menu_book</mat-icon>
                        <div>
                          <span class="profile-details__label">Course</span>
                          <span>{{ profile()!.courseOfStudy }}</span>
                          @if (profile()!.graduationYear) {
                            <span class="text-muted"> ({{ profile()!.graduationYear }})</span>
                          }
                        </div>
                      </div>
                    }
                    @if (profile()!.linkedInUrl) {
                      <div class="profile-details__item">
                        <mat-icon aria-hidden="true">link</mat-icon>
                        <div>
                          <span class="profile-details__label">LinkedIn</span>
                          <a [href]="profile()!.linkedInUrl" target="_blank" rel="noopener">
                            {{ profile()!.linkedInUrl }}
                          </a>
                        </div>
                      </div>
                    }
                    @if (profile()!.gitHubUrl) {
                      <div class="profile-details__item">
                        <mat-icon aria-hidden="true">code</mat-icon>
                        <div>
                          <span class="profile-details__label">GitHub</span>
                          <a [href]="profile()!.gitHubUrl" target="_blank" rel="noopener">
                            {{ profile()!.gitHubUrl }}
                          </a>
                        </div>
                      </div>
                    }
                  </div>
                  @if (profile()!.bio) {
                    <mat-divider class="profile-details__divider" />
                    <p class="profile-details__bio">{{ profile()!.bio }}</p>
                  }
                </div>
              }
            </mat-card-content>
            @if (!editingProfile()) {
              <mat-card-actions align="end">
                <button mat-button color="primary" (click)="startEdit()">
                  <mat-icon>edit</mat-icon>
                  Edit Profile
                </button>
              </mat-card-actions>
            }
          </mat-card>

          <!-- Education -->
          <mat-card>
            <mat-card-header>
              <mat-card-title>Education</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              @for (edu of profile()!.educations; track edu.id) {
                <div class="section-item">
                  <div class="section-item__content">
                    <h3 class="section-item__title">{{ edu.institution }}</h3>
                    <p class="section-item__subtitle">
                      {{ edu.degree }} · {{ edu.fieldOfStudy }}
                    </p>
                    <p class="section-item__meta text-muted">
                      {{ edu.startYear }} — {{ edu.isCurrent ? 'Present' : edu.endYear }}
                      @if (edu.grade) { · GPA/Grade: {{ edu.grade }} }
                    </p>
                  </div>
                  <div class="section-item__actions">
                    <button mat-icon-button (click)="startEditEducation(edu)" aria-label="Edit education">
                      <mat-icon>edit</mat-icon>
                    </button>
                    <button mat-icon-button color="warn" (click)="deleteEducation(edu.id)" aria-label="Delete education">
                      <mat-icon>delete</mat-icon>
                    </button>
                  </div>
                </div>
                <mat-divider />
              }

              @if (addingEducation()) {
                <form [formGroup]="educationForm" (ngSubmit)="saveEducation()" class="profile-form section-form">
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>Institution</mat-label>
                      <input matInput formControlName="institution" />
                      @if (educationForm.get('institution')?.hasError('required')) {
                        <mat-error>Required.</mat-error>
                      }
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>Degree</mat-label>
                      <input matInput formControlName="degree" />
                    </mat-form-field>
                  </div>
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>Field of Study</mat-label>
                      <input matInput formControlName="fieldOfStudy" />
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>Start Year</mat-label>
                      <input matInput type="number" formControlName="startYear" />
                    </mat-form-field>
                  </div>
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>End Year</mat-label>
                      <input matInput type="number" formControlName="endYear" />
                      <mat-hint>Leave blank if current</mat-hint>
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>GPA / Percentage</mat-label>
                      <input matInput type="number" step="0.01" formControlName="grade" />
                    </mat-form-field>
                  </div>
                  <div class="profile-form__actions">
                    <button mat-button type="button" (click)="cancelEducation()">Cancel</button>
                    <button mat-raised-button color="primary" type="submit" [disabled]="savingSection()">
                      Save
                    </button>
                  </div>
                </form>
              }
            </mat-card-content>
            <mat-card-actions>
              <button mat-button color="primary" (click)="startAddEducation()">
                <mat-icon>add</mat-icon>
                Add Education
              </button>
            </mat-card-actions>
          </mat-card>

          <!-- Experience -->
          <mat-card>
            <mat-card-header>
              <mat-card-title>Work Experience</mat-card-title>
            </mat-card-header>
            <mat-card-content>
              @for (exp of profile()!.experiences; track exp.id) {
                <div class="section-item">
                  <div class="section-item__content">
                    <h3 class="section-item__title">{{ exp.title }}</h3>
                    <p class="section-item__subtitle">{{ exp.company }}</p>
                    <p class="section-item__meta text-muted">
                      {{ exp.startDate | date:'MMM yyyy' }} —
                      {{ exp.isCurrent ? 'Present' : (exp.endDate | date:'MMM yyyy') }}
                    </p>
                    @if (exp.description) {
                      <p class="section-item__description">{{ exp.description }}</p>
                    }
                  </div>
                  <div class="section-item__actions">
                    <button mat-icon-button (click)="startEditExperience(exp)" aria-label="Edit experience">
                      <mat-icon>edit</mat-icon>
                    </button>
                    <button mat-icon-button color="warn" (click)="deleteExperience(exp.id)" aria-label="Delete experience">
                      <mat-icon>delete</mat-icon>
                    </button>
                  </div>
                </div>
                <mat-divider />
              }

              @if (addingExperience()) {
                <form [formGroup]="experienceForm" (ngSubmit)="saveExperience()" class="profile-form section-form">
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>Job Title</mat-label>
                      <input matInput formControlName="title" />
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>Company</mat-label>
                      <input matInput formControlName="company" />
                    </mat-form-field>
                  </div>
                  <div class="profile-form__row">
                    <mat-form-field appearance="outline">
                      <mat-label>Start Date</mat-label>
                      <input matInput type="date" formControlName="startDate" />
                    </mat-form-field>
                    <mat-form-field appearance="outline">
                      <mat-label>End Date</mat-label>
                      <input matInput type="date" formControlName="endDate" />
                      <mat-hint>Leave blank if current</mat-hint>
                    </mat-form-field>
                  </div>
                  <mat-form-field appearance="outline" class="profile-form__full">
                    <mat-label>Description</mat-label>
                    <textarea matInput formControlName="description" rows="3"></textarea>
                  </mat-form-field>
                  <div class="profile-form__actions">
                    <button mat-button type="button" (click)="cancelExperience()">Cancel</button>
                    <button mat-raised-button color="primary" type="submit" [disabled]="savingSection()">
                      Save
                    </button>
                  </div>
                </form>
              }
            </mat-card-content>
            <mat-card-actions>
              <button mat-button color="primary" (click)="startAddExperience()">
                <mat-icon>add</mat-icon>
                Add Experience
              </button>
            </mat-card-actions>
          </mat-card>

        </div>
      }
    </div>
  `,
  styleUrl: './student-profile.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StudentProfileComponent implements OnInit {
  private readonly service = inject(ProfileService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly profile = signal<StudentProfile | null>(null);
  readonly editingProfile = signal(false);
  readonly savingProfile = signal(false);
  readonly addingEducation = signal(false);
  readonly editingEducationId = signal<number | null>(null);
  readonly addingExperience = signal(false);
  readonly editingExperienceId = signal<number | null>(null);
  readonly savingSection = signal(false);

  readonly profileForm: FormGroup = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    phoneNumber: [null],
    dateOfBirth: [null],
    gender: [null],
    currentInstitution: [null],
    courseOfStudy: [null],
    graduationYear: [null],
    gpa: [null],
    bio: [null, Validators.maxLength(1000)],
    linkedInUrl: [null],
    gitHubUrl: [null],
    portfolioUrl: [null],
  });

  readonly educationForm: FormGroup = this.fb.group({
    institution: ['', Validators.required],
    degree: ['', Validators.required],
    fieldOfStudy: ['', Validators.required],
    startYear: [null, [Validators.required, Validators.min(1950), Validators.max(2100)]],
    endYear: [null],
    isCurrent: [false],
    grade: [null],
  });

  readonly experienceForm: FormGroup = this.fb.group({
    title: ['', Validators.required],
    company: ['', Validators.required],
    description: [null],
    startDate: ['', Validators.required],
    endDate: [null],
    isCurrent: [false],
  });

  ngOnInit(): void {
    this.loadProfile();
  }

  private loadProfile(): void {
    this.loading.set(true);
    this.service
      .getProfile()
      .pipe(catchError(() => {
        this.loading.set(false);
        return EMPTY;
      }))
      .subscribe((res) => {
        this.profile.set(res.data);
        this.loading.set(false);
      });
  }

  startEdit(): void {
    const p = this.profile();
    if (!p) return;
    this.profileForm.patchValue({
      firstName: p.firstName,
      lastName: p.lastName,
      phoneNumber: p.phoneNumber,
      dateOfBirth: p.dateOfBirth,
      gender: p.gender,
      currentInstitution: p.currentInstitution,
      courseOfStudy: p.courseOfStudy,
      graduationYear: p.graduationYear,
      gpa: p.gpa,
      bio: p.bio,
      linkedInUrl: p.linkedInUrl,
      gitHubUrl: p.gitHubUrl,
      portfolioUrl: p.portfolioUrl,
    });
    this.editingProfile.set(true);
  }

  cancelEdit(): void {
    this.editingProfile.set(false);
    this.profileForm.reset();
  }

  saveProfile(): void {
    if (this.profileForm.invalid) return;
    this.savingProfile.set(true);

    this.service
      .updateProfile(this.profileForm.value)
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to save profile.', 'Dismiss', { duration: 4000 });
        this.savingProfile.set(false);
        return EMPTY;
      }))
      .subscribe(() => {
        this.savingProfile.set(false);
        this.editingProfile.set(false);
        this.snackBar.open('Profile saved.', undefined, { duration: 3000 });
        this.loadProfile();
      });
  }

  startAddEducation(): void {
    this.educationForm.reset({ isCurrent: false });
    this.editingEducationId.set(null);
    this.addingEducation.set(true);
  }

  startEditEducation(edu: Education): void {
    this.educationForm.patchValue(edu);
    this.editingEducationId.set(edu.id);
    this.addingEducation.set(true);
  }

  cancelEducation(): void {
    this.addingEducation.set(false);
    this.editingEducationId.set(null);
  }

  saveEducation(): void {
    if (this.educationForm.invalid) return;
    this.savingSection.set(true);

    const data: AddEducationForm = this.educationForm.value;
    const editId = this.editingEducationId();

    const req$: Observable<unknown> = editId
      ? this.service.updateEducation(editId, data)
      : this.service.addEducation(data);

    req$
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to save education.', 'Dismiss', { duration: 4000 });
        this.savingSection.set(false);
        return EMPTY;
      }))
      .subscribe(() => {
        this.savingSection.set(false);
        this.addingEducation.set(false);
        this.editingEducationId.set(null);
        this.loadProfile();
      });
  }

  deleteEducation(id: number): void {
    this.service
      .deleteEducation(id)
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to delete.', 'Dismiss', { duration: 4000 });
        return EMPTY;
      }))
      .subscribe(() => {
        this.snackBar.open('Education removed.', undefined, { duration: 2000 });
        this.loadProfile();
      });
  }

  startAddExperience(): void {
    this.experienceForm.reset({ isCurrent: false });
    this.editingExperienceId.set(null);
    this.addingExperience.set(true);
  }

  startEditExperience(exp: Experience): void {
    this.experienceForm.patchValue(exp);
    this.editingExperienceId.set(exp.id);
    this.addingExperience.set(true);
  }

  cancelExperience(): void {
    this.addingExperience.set(false);
    this.editingExperienceId.set(null);
  }

  saveExperience(): void {
    if (this.experienceForm.invalid) return;
    this.savingSection.set(true);

    const data: AddExperienceForm = this.experienceForm.value;
    const editId = this.editingExperienceId();

    const req$: Observable<unknown> = editId
      ? this.service.updateExperience(editId, data)
      : this.service.addExperience(data);

    req$
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to save experience.', 'Dismiss', { duration: 4000 });
        this.savingSection.set(false);
        return EMPTY;
      }))
      .subscribe(() => {
        this.savingSection.set(false);
        this.addingExperience.set(false);
        this.editingExperienceId.set(null);
        this.loadProfile();
      });
  }

  deleteExperience(id: number): void {
    this.service
      .deleteExperience(id)
      .pipe(catchError((err) => {
        this.snackBar.open(err.error?.message ?? 'Failed to delete.', 'Dismiss', { duration: 4000 });
        return EMPTY;
      }))
      .subscribe(() => {
        this.snackBar.open('Experience removed.', undefined, { duration: 2000 });
        this.loadProfile();
      });
  }
}
