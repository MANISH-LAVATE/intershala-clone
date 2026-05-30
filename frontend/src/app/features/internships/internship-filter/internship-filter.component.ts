import {
  Component,
  ChangeDetectionStrategy,
  inject,
  OnInit,
  signal,
  output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSliderModule } from '@angular/material/slider';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { LookupService } from '../../../core/services/lookup.service';
import { InternshipFilters } from '../models/internship.model';

@Component({
  selector: 'app-internship-filter',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatSliderModule,
    MatButtonModule,
    MatExpansionModule,
  ],
  template: `
    <aside class="filter-sidebar" aria-label="Filter internships">
      <div class="filter-sidebar__header">
        <h2 class="filter-sidebar__title">Filters</h2>
        <button mat-button color="primary" (click)="onClear()" [disabled]="!hasActiveFilters()">
          Clear All
        </button>
      </div>

      <form [formGroup]="form" class="filter-sidebar__form">
        <!-- Work from home -->
        <div class="filter-sidebar__section">
          <mat-checkbox formControlName="isRemote" (change)="onFilterChange()">
            Work from Home
          </mat-checkbox>
        </div>

        <!-- Category -->
        <mat-expansion-panel expanded>
          <mat-expansion-panel-header>
            <mat-panel-title>Profile / Category</mat-panel-title>
          </mat-expansion-panel-header>
          <mat-form-field>
            <mat-label>Category</mat-label>
            <mat-select formControlName="categoryId" (selectionChange)="onFilterChange()">
              <mat-option [value]="null">All Categories</mat-option>
              @for (cat of lookupService.categories(); track cat.id) {
                <mat-option [value]="cat.id">{{ cat.name }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        </mat-expansion-panel>

        <!-- Location -->
        <mat-expansion-panel expanded>
          <mat-expansion-panel-header>
            <mat-panel-title>Location</mat-panel-title>
          </mat-expansion-panel-header>
          <mat-form-field>
            <mat-label>City</mat-label>
            <mat-select formControlName="locationId" (selectionChange)="onFilterChange()">
              <mat-option [value]="null">All Locations</mat-option>
              @for (loc of lookupService.locations(); track loc.id) {
                <mat-option [value]="loc.id">{{ loc.cityName }}</mat-option>
              }
            </mat-select>
          </mat-form-field>
        </mat-expansion-panel>

        <!-- Stipend -->
        <mat-expansion-panel expanded>
          <mat-expansion-panel-header>
            <mat-panel-title>Minimum Stipend</mat-panel-title>
          </mat-expansion-panel-header>
          <mat-form-field>
            <mat-label>Min Stipend (₹/month)</mat-label>
            <mat-select formControlName="stipendMin" (selectionChange)="onFilterChange()">
              <mat-option [value]="null">Any</mat-option>
              <mat-option [value]="2000">₹2,000+</mat-option>
              <mat-option [value]="5000">₹5,000+</mat-option>
              <mat-option [value]="10000">₹10,000+</mat-option>
              <mat-option [value]="15000">₹15,000+</mat-option>
              <mat-option [value]="20000">₹20,000+</mat-option>
              <mat-option [value]="30000">₹30,000+</mat-option>
            </mat-select>
          </mat-form-field>
        </mat-expansion-panel>

        <!-- Duration -->
        <mat-expansion-panel>
          <mat-expansion-panel-header>
            <mat-panel-title>Duration</mat-panel-title>
          </mat-expansion-panel-header>
          <mat-form-field>
            <mat-label>Max Duration</mat-label>
            <mat-select formControlName="durationMonths" (selectionChange)="onFilterChange()">
              <mat-option [value]="null">Any Duration</mat-option>
              <mat-option [value]="1">1 Month</mat-option>
              <mat-option [value]="2">Up to 2 Months</mat-option>
              <mat-option [value]="3">Up to 3 Months</mat-option>
              <mat-option [value]="6">Up to 6 Months</mat-option>
            </mat-select>
          </mat-form-field>
        </mat-expansion-panel>

        <!-- Type -->
        <mat-expansion-panel>
          <mat-expansion-panel-header>
            <mat-panel-title>Internship Type</mat-panel-title>
          </mat-expansion-panel-header>
          <mat-form-field>
            <mat-label>Type</mat-label>
            <mat-select formControlName="internshipType" (selectionChange)="onFilterChange()">
              <mat-option [value]="null">All Types</mat-option>
              <mat-option value="InOffice">In Office</mat-option>
              <mat-option value="Remote">Work from Home</mat-option>
              <mat-option value="Hybrid">Hybrid</mat-option>
            </mat-select>
          </mat-form-field>
        </mat-expansion-panel>
      </form>
    </aside>
  `,
  styleUrl: './internship-filter.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InternshipFilterComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly lookupService = inject(LookupService);

  readonly filtersChanged = output<Partial<InternshipFilters>>();
  readonly cleared = output<void>();

  readonly hasActiveFilters = signal(false);

  readonly form = this.fb.group({
    isRemote: [false],
    categoryId: [null as number | null],
    locationId: [null as number | null],
    stipendMin: [null as number | null],
    durationMonths: [null as number | null],
    internshipType: [null as string | null],
  });

  ngOnInit(): void {
    this.lookupService.loadCategories();
    this.lookupService.loadLocations();
  }

  onFilterChange(): void {
    const val = this.form.getRawValue();
    this.hasActiveFilters.set(
      !!val.categoryId || !!val.locationId || !!val.stipendMin ||
      val.isRemote || !!val.durationMonths || !!val.internshipType,
    );
    this.filtersChanged.emit({
      categoryId: val.categoryId ?? undefined,
      locationId: val.locationId ?? undefined,
      stipendMin: val.stipendMin ?? undefined,
      isRemote: val.isRemote ?? false,
      durationMonths: val.durationMonths ?? undefined,
      internshipType: (val.internshipType as InternshipFilters['internshipType']) ?? undefined,
    });
  }

  onClear(): void {
    this.form.reset({ isRemote: false, categoryId: null, locationId: null, stipendMin: null, durationMonths: null, internshipType: null });
    this.hasActiveFilters.set(false);
    this.cleared.emit();
  }
}
