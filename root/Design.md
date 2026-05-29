# Design.md — Internshala Clone: UI/UX Design System

## Design System Overview

This design system is inspired by Internshala's visual identity, modernized with Material Design 3 principles. All values are implemented as SCSS variables and Angular Material theme tokens.

**Design Philosophy**: Clean, trustworthy, student-friendly. Prioritize information density without visual clutter. Use color purposefully to communicate status, urgency, and hierarchy.

---

## Typography System

### Font Families

```scss
// _typography.scss
$font-family-primary: 'Inter', 'Roboto', -apple-system, BlinkMacSystemFont, sans-serif;
$font-family-heading: 'Inter', 'Poppins', sans-serif;
$font-family-mono: 'JetBrains Mono', 'Fira Code', 'Courier New', monospace;
```

### Type Scale

| Token | Size | Line Height | Weight | Usage |
|---|---|---|---|---|
| `--text-display` | 48px / 3rem | 1.1 | 700 | Hero headlines |
| `--text-h1` | 36px / 2.25rem | 1.2 | 700 | Page titles |
| `--text-h2` | 28px / 1.75rem | 1.25 | 600 | Section headings |
| `--text-h3` | 22px / 1.375rem | 1.3 | 600 | Card headings |
| `--text-h4` | 18px / 1.125rem | 1.4 | 600 | Subsection labels |
| `--text-body-lg` | 16px / 1rem | 1.6 | 400 | Primary body text |
| `--text-body` | 14px / 0.875rem | 1.5 | 400 | Secondary body |
| `--text-body-sm` | 13px / 0.8125rem | 1.5 | 400 | Supporting text |
| `--text-caption` | 12px / 0.75rem | 1.4 | 400 | Labels, captions |
| `--text-overline` | 11px / 0.6875rem | 1.4 | 600 | Tags, overlines (uppercase) |

### SCSS Typography Mixin

```scss
// Usage: @include typography('h2')
@mixin typography($level) {
  font-family: $font-family-heading;
  font-size: map.get($type-scale, $level, 'size');
  line-height: map.get($type-scale, $level, 'line-height');
  font-weight: map.get($type-scale, $level, 'weight');
  letter-spacing: map.get($type-scale, $level, 'letter-spacing');
}
```

---

## Color Palette

### Brand Colors

```scss
// Primary — Internshala Blue
$color-primary-50:  #e3f2fd;
$color-primary-100: #bbdefb;
$color-primary-200: #90caf9;
$color-primary-300: #64b5f6;
$color-primary-400: #42a5f5;
$color-primary-500: #2196f3;  // Primary brand color
$color-primary-600: #1e88e5;  // Hover state
$color-primary-700: #1976d2;  // Active state
$color-primary-800: #1565c0;  // Dark
$color-primary-900: #0d47a1;  // Darkest

// Accent — Success Green (used for "Active", "Open" statuses)
$color-accent-500:  #4caf50;
$color-accent-600:  #43a047;
$color-accent-700:  #388e3c;

// Warning — Amber
$color-warning-500: #ff9800;
$color-warning-600: #fb8c00;

// Error — Red
$color-error-500:   #f44336;
$color-error-600:   #e53935;

// Internship Featured/Premium
$color-premium-500: #ff6b35;  // Orange — for featured listings
$color-premium-600: #e55f2e;
```

### Neutral Colors

```scss
$color-neutral-0:   #ffffff;
$color-neutral-50:  #fafafa;
$color-neutral-100: #f5f5f5;
$color-neutral-200: #eeeeee;
$color-neutral-300: #e0e0e0;
$color-neutral-400: #bdbdbd;
$color-neutral-500: #9e9e9e;
$color-neutral-600: #757575;
$color-neutral-700: #616161;
$color-neutral-800: #424242;
$color-neutral-900: #212121;
```

### Semantic Tokens

```scss
// Light Mode
$color-background:        $color-neutral-50;
$color-surface:           $color-neutral-0;
$color-surface-variant:   $color-neutral-100;
$color-on-surface:        $color-neutral-900;
$color-on-surface-muted:  $color-neutral-600;
$color-border:            $color-neutral-200;
$color-border-strong:     $color-neutral-300;

// Status Colors
$color-status-active:     #e8f5e9;  // bg
$color-status-active-fg:  #2e7d32;  // text
$color-status-closed:     #fce4ec;
$color-status-closed-fg:  #c62828;
$color-status-pending:    #fff8e1;
$color-status-pending-fg: #f57f17;
$color-status-draft:      #eeeeee;
$color-status-draft-fg:   #616161;
```

### Angular Material Theme

```scss
// custom-theme.scss
@use '@angular/material' as mat;

$primary-palette: mat.define-palette(mat.$blue-palette, 500, 300, 700);
$accent-palette: mat.define-palette(mat.$green-palette, 500, 300, 700);
$warn-palette: mat.define-palette(mat.$red-palette);

$light-theme: mat.define-light-theme((
  color: (
    primary: $primary-palette,
    accent: $accent-palette,
    warn: $warn-palette
  ),
  typography: mat.define-typography-config(
    $font-family: 'Inter, Roboto, sans-serif'
  ),
  density: 0
));
```

---

## UI Spacing System

Based on an 8px grid system:

```scss
$space-0:   0px;
$space-1:   4px;    // --space-1
$space-2:   8px;    // --space-2  (base unit)
$space-3:   12px;   // --space-3
$space-4:   16px;   // --space-4
$space-5:   20px;   // --space-5
$space-6:   24px;   // --space-6
$space-8:   32px;   // --space-8
$space-10:  40px;   // --space-10
$space-12:  48px;   // --space-12
$space-16:  64px;   // --space-16
$space-20:  80px;   // --space-20
$space-24:  96px;   // --space-24

// Component-specific
$border-radius-sm:  4px;
$border-radius-md:  8px;
$border-radius-lg:  12px;
$border-radius-xl:  16px;
$border-radius-full: 9999px;

$shadow-sm: 0 1px 3px rgba(0,0,0,0.08), 0 1px 2px rgba(0,0,0,0.06);
$shadow-md: 0 4px 6px rgba(0,0,0,0.07), 0 2px 4px rgba(0,0,0,0.06);
$shadow-lg: 0 10px 15px rgba(0,0,0,0.08), 0 4px 6px rgba(0,0,0,0.05);
$shadow-xl: 0 20px 25px rgba(0,0,0,0.1), 0 10px 10px rgba(0,0,0,0.04);
```

---

## Responsive Breakpoints

```scss
// _breakpoints.scss
$breakpoints: (
  'xs': 0px,       // Mobile portrait (< 600px)
  'sm': 600px,     // Mobile landscape / tablet portrait
  'md': 960px,     // Tablet landscape
  'lg': 1280px,    // Desktop
  'xl': 1920px     // Wide desktop
);

// Mixins
@mixin respond-to($breakpoint) {
  $value: map.get($breakpoints, $breakpoint);
  @media (min-width: $value) { @content; }
}

// Usage:
.internship-grid {
  display: grid;
  grid-template-columns: 1fr;                    // xs: 1 column

  @include respond-to('sm') {
    grid-template-columns: repeat(2, 1fr);        // sm: 2 columns
  }

  @include respond-to('lg') {
    grid-template-columns: repeat(3, 1fr);        // lg: 3 columns
  }
}
```

### Container Max-Widths

| Breakpoint | Container Width |
|---|---|
| xs | 100% (16px padding) |
| sm | 100% (24px padding) |
| md | 100% (32px padding) |
| lg | 1200px |
| xl | 1400px |

---

## Reusable UI Components

### 1. InternshipCard Component

```
┌─────────────────────────────────────────────────────┐
│  [Company Logo 48x48]  Company Name          [★ Save]│
│                        Location · Work Type          │
│  Internship Title (h3)                               │
│  ─────────────────────────────────────────────────  │
│  ₹ X,000-Y,000/month  │  Z months  │  N openings    │
│  ─────────────────────────────────────────────────  │
│  [Skill Badge] [Skill Badge] [Skill Badge]           │
│  Posted 2 days ago                    [Apply Now →]  │
└─────────────────────────────────────────────────────┘
```

**States**: Default, Hover (elevated shadow), Featured (premium border), Applied (green checkmark), Saved (filled star), Loading (skeleton).

### 2. FilterSidebar Component

```
┌──────────────────────────┐
│ Filters          [Clear] │
├──────────────────────────┤
│ ▼ Profile Type           │
│   ● Engineering/IT       │
│   ○ Marketing            │
│   ○ Design               │
├──────────────────────────┤
│ ▼ Location               │
│   [Search locations...]  │
│   ☑ Bangalore            │
│   ☑ Mumbai               │
│   ☐ Delhi                │
├──────────────────────────┤
│ ▼ Stipend (₹/month)      │
│   [════○════] 5,000      │
├──────────────────────────┤
│ ▼ Duration               │
│   ☑ 1-3 months           │
│   ☑ 3-6 months           │
│   ☐ 6+ months            │
├──────────────────────────┤
│ ▼ Work From Home         │
│   [Toggle: ON]           │
└──────────────────────────┘
```

### 3. ApplicationStatusBadge

```
Status Variants:
[● Applied]        — Blue background
[● Under Review]   — Amber background
[● Shortlisted]    — Purple background
[✓ Selected]       — Green background
[✗ Rejected]       — Red background
[◌ Withdrawn]      — Grey background
```

### 4. DataTable Component (Admin/Employer)

```
┌──────────────────────────────────────────────────────────────┐
│ Search... [          ]    Filters ▼    [+ Add New]    Export ▼│
├────┬─────────────────┬──────────┬────────────┬──────────────-┤
│ ☐  │ Applicant       │ Position │ Applied On │ Status        │
├────┼─────────────────┼──────────┼────────────┼───────────────┤
│ ☑  │ Rahul Sharma    │ SDE Intern│ 15 Jan    │ [Shortlisted] │
│ ☐  │ Priya Singh     │ SDE Intern│ 16 Jan    │ [Applied]     │
├────┴─────────────────┴──────────┴────────────┴───────────────┤
│ Showing 1-20 of 124    [← Prev]  1 2 3 ... 7  [Next →]       │
└──────────────────────────────────────────────────────────────┘
```

### 5. Reusable Button Variants

```scss
// Variants: primary | secondary | outline | ghost | danger | icon
// Sizes: sm | md | lg
// States: default | hover | active | disabled | loading

.btn-primary {
  background: $color-primary-500;
  color: white;
  border-radius: $border-radius-full;
  padding: $space-2 $space-6;
  font-weight: 600;
  transition: all 200ms ease;

  &:hover { background: $color-primary-600; transform: translateY(-1px); }
  &:active { background: $color-primary-700; transform: translateY(0); }
  &:disabled { opacity: 0.5; cursor: not-allowed; transform: none; }
  &.loading { pointer-events: none; }
}
```

---

## Dashboard Layout Rules

### Student Dashboard Layout

```
┌────────────────────────────────────────────────────────────────┐
│ HEADER: Logo | Search Bar | Notifications | Profile Menu        │
├────────────────────────────────────────────────────────────────┤
│ SIDEBAR (240px)    │  MAIN CONTENT                              │
│ ─────────────────  │  ─────────────────────────────────────    │
│ Dashboard          │  ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐    │
│ Applications       │  │Apps  │ │Saved │ │Views │ │Score │    │
│ Saved Items        │  │  12  │ │  8   │ │  45  │ │  78% │    │
│ Resume Builder     │  └──────┘ └──────┘ └──────┘ └──────┘    │
│ ─────────────────  │                                           │
│ Find Internships   │  Recommended Internships ─────────────── │
│ Find Jobs          │  [Card] [Card] [Card]                     │
│ Courses            │                                           │
│ ─────────────────  │  Recent Applications ─────────────────── │
│ Settings           │  [Table Row] [Table Row] [Table Row]      │
│ Help               │                                           │
└────────────────────┴────────────────────────────────────────────┘
```

### Layout Rules

- Sidebar: fixed 240px on desktop; off-canvas drawer on mobile (< 960px).
- Main content: `calc(100vw - 240px)` on desktop; full-width on mobile.
- Header: `position: sticky; top: 0; z-index: 100;` — height 64px.
- Content padding: 32px desktop, 16px mobile.
- Stat cards: 4-column grid (desktop) → 2-column (tablet) → 1-column (mobile).

---

## Form Design Standards

### Input Field Anatomy

```
Label (required indicator *)
┌─────────────────────────────────┐
│ Placeholder text                │  ← Focus: primary border + shadow
└─────────────────────────────────┘
  Helper text or error message
```

### Form Rules

- All form fields use Angular Material components (`mat-form-field`, `mat-input`, etc.).
- Appearance: `outline` (preferred) or `fill`.
- Required fields marked with asterisk in label.
- Error messages appear below field (not tooltips).
- Error messages are specific: "Email already registered" not "Invalid input".
- Group related fields with visual separation (8px gap within group, 24px between groups).
- Submit button at bottom-right of form (or full-width on mobile).
- Disable submit button while submitting; show spinner inside button.
- Inline validation on blur; re-validate on input after first error.

### Form Layout

- Single column for mobile, two columns for desktop where appropriate.
- Labels always above inputs (never floating labels for complex forms — accessibility).
- Character count shown for text areas.
- Maximum form width: 640px for focused forms, full-width for complex dashboards.

---

## Modal Standards

### Modal Anatomy

```
┌─────────────────────────────────────────────────────┐
│ Modal Title                                    [✕]   │
├─────────────────────────────────────────────────────┤
│                                                       │
│  Modal body content                                   │
│  (scrollable if content overflows)                    │
│                                                       │
├─────────────────────────────────────────────────────┤
│                    [Cancel]  [Confirm Action]         │
└─────────────────────────────────────────────────────┘
```

### Modal Rules

- Use Angular CDK Dialog or Angular Material MatDialog.
- Small: max-width 400px (confirmations, simple forms).
- Medium: max-width 600px (standard forms, detail views).
- Large: max-width 800px (complex forms, preview modals).
- Full-screen on mobile (< 600px) via `panelClass`.
- Focus trap inside modal (CDK FocusTrap).
- Close on ESC, close on backdrop click (configurable).
- Scroll: modal body scrolls, header and footer are sticky.
- Destructive actions use `btn-danger` and require explicit confirmation text.

---

## Table Standards

### Standard Data Table

- Use Angular Material `mat-table` with `mat-sort` and `mat-paginator`.
- Default page size: 20. Options: 10, 20, 50, 100.
- Fixed header on scroll.
- Row hover: `$color-primary-50` background.
- Selected row: `$color-primary-100` background.
- Sticky first column for wide tables with horizontal scroll.
- Responsive: on mobile, table collapses to card list view.
- Empty state: illustration + message + CTA button.
- Loading state: skeleton rows (3 rows visible during load).

---

## Sidebar Standards

### Navigation Sidebar

- Width: 240px (expanded), 64px (collapsed icon-only mode).
- Collapse toggle button at bottom of sidebar.
- Active route: primary color background, bold text, left accent border.
- Icons: Material Symbols (outlined style, 24px).
- Groups separated by 1px divider with section label.
- Mobile: off-canvas drawer (matDrawer, mode=over).
- Animation: slide-in 200ms ease.

### Filter Sidebar

- Width: 280px, right-aligned on listing pages.
- Collapsible filter groups with smooth height animation.
- "Apply Filters" button sticky at bottom.
- "Clear All" link at top.
- Active filter count badge on mobile toggle button.

---

## Header/Footer Standards

### Header (AppHeaderComponent)

**Desktop (≥ 960px)**:
```
[Logo]  [Nav: Internships | Jobs | Courses | For Employers]
        [Search Input (expandable)]  [Notifications 🔔(3)]
        [Profile Avatar ▾] → dropdown
```

**Mobile (< 960px)**:
```
[☰ Menu]  [Logo]  [🔔]  [Avatar]
```

Rules:
- Height: 64px, `position: sticky`.
- Background: white with `$shadow-sm` on scroll (add class via scroll listener).
- Search: expands to overlay on mobile (full-width, auto-focus).
- Notifications dropdown: max 5 recent, "View All" link.
- Profile dropdown: My Profile, My Applications, Settings, Logout.

### Footer (AppFooterComponent)

```
┌────────────────────────────────────────────────────────┐
│ [Logo]  Helping students achieve their dream career     │
│                                                          │
│ Company      Students      Employers    Support          │
│ About Us     Internships   Post Jobs    Help Center      │
│ Blog         Jobs          Our Services Contact          │
│ Careers      Courses       Pricing      Privacy          │
│                                                          │
│ ─────────────────────────────────────────────────────  │
│ © 2024 Internshala Clone   [Social Icons]               │
└────────────────────────────────────────────────────────┘
```

---

## Accessibility Guidelines

### Core Requirements (WCAG 2.1 AA)

1. **Color Contrast**: Minimum 4.5:1 for normal text, 3:1 for large text (>18px or >14px bold).
2. **Focus Visible**: All focusable elements have visible focus rings (2px solid `$color-primary-500`).
3. **Keyboard Navigation**: All features operable without mouse.
4. **Screen Reader**: Meaningful content order, `aria-label` where needed, `role` attributes where semantic HTML is insufficient.
5. **Form Accessibility**: Labels programmatically associated, errors announced via `aria-live`.

### ARIA Usage

```html
<!-- Internship card with meaningful roles -->
<article class="internship-card" role="article" aria-label="Software Developer Intern at Google">
  <!-- Status badge -->
  <span class="badge badge--active" role="status" aria-label="Actively accepting applications">
    Actively hiring
  </span>
  <!-- Apply button -->
  <button mat-raised-button aria-label="Apply for Software Developer Intern at Google">
    Apply Now
  </button>
</article>

<!-- Loading state -->
<div aria-busy="true" aria-label="Loading internships...">
  <app-skeleton-loader />
</div>
```

---

## Mobile-First Design Rules

1. All SCSS starts with mobile styles; desktop overrides use `@include respond-to('md')`.
2. Touch targets: minimum 44x44px (never smaller than 40x40px).
3. No hover-only interactions — all hover effects also work on tap.
4. Bottom navigation bar on mobile for primary routes (Explore, Applications, Saved, Profile).
5. Full-screen modals on mobile (< 600px).
6. Avoid fixed elements that overlap content on mobile.
7. Form inputs font-size ≥ 16px on mobile (prevents iOS auto-zoom).
8. Images use `aspect-ratio` to prevent layout shift.

---

## Animation Standards

```scss
// _animations.scss
$duration-fast:   100ms;
$duration-normal: 200ms;
$duration-slow:   300ms;
$duration-page:   400ms;

$easing-standard: cubic-bezier(0.4, 0, 0.2, 1);   // Material standard
$easing-enter:    cubic-bezier(0, 0, 0.2, 1);      // Decelerate (elements entering)
$easing-exit:     cubic-bezier(0.4, 0, 1, 1);      // Accelerate (elements leaving)
```

### Animation Rules

- Respect `prefers-reduced-motion` — wrap all animations:
  ```scss
  @media (prefers-reduced-motion: no-preference) { /* animations */ }
  ```
- Page transitions: 300ms fade-in-slide (Angular route animations).
- Component enter: 200ms fade + scale from 0.98.
- List items: staggered 50ms delay per item (max 10 items staggered).
- Hover transitions: 150ms for color/shadow changes.
- Never animate `width`, `height`, or `top/left` — use `transform` and `opacity`.

---

## Loading States

### Skeleton Loaders

Match the exact layout of the content being loaded. No generic spinners for page-level content.

```html
<!-- InternshipCardSkeleton -->
<div class="skeleton-card" aria-busy="true" aria-label="Loading internship...">
  <div class="skeleton-circle" style="width:48px;height:48px"></div>
  <div class="skeleton-lines">
    <div class="skeleton-line" style="width:60%"></div>
    <div class="skeleton-line" style="width:40%"></div>
  </div>
</div>
```

```scss
.skeleton-line {
  background: linear-gradient(90deg,
    $color-neutral-200 25%,
    $color-neutral-100 50%,
    $color-neutral-200 75%
  );
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite;
  border-radius: $border-radius-sm;
  height: 14px;
  margin-bottom: $space-2;
}

@keyframes shimmer {
  0% { background-position: -200% 0; }
  100% { background-position: 200% 0; }
}
```

### Button Loading State

```html
<button [disabled]="isLoading()">
  <mat-spinner *ngIf="isLoading()" diameter="18" />
  <span [hidden]="isLoading()">Apply Now</span>
</button>
```

---

## Empty States

Every list or data view must have a designed empty state.

```
┌───────────────────────────────────────────────────┐
│                                                     │
│              [Illustration SVG]                     │
│                                                     │
│         No internships found                        │
│   Try adjusting your filters or search terms        │
│                                                     │
│              [Clear Filters]                        │
│                                                     │
└───────────────────────────────────────────────────┘
```

**Empty State Types**:
1. **No search results**: "No results for '{query}'" + clear search CTA.
2. **No applications yet**: "Start exploring internships" + explore CTA.
3. **No notifications**: "You're all caught up!" + no CTA needed.
4. **First-time user**: Onboarding-style with action CTA.
5. **Permissions required**: Prompt to complete profile + action CTA.

---

## Error States

### Inline Form Errors

```html
<mat-form-field appearance="outline">
  <mat-label>Email</mat-label>
  <input matInput formControlName="email" type="email">
  <mat-error>
    @if (form.get('email')?.hasError('required')) { Email is required }
    @if (form.get('email')?.hasError('email')) { Enter a valid email address }
    @if (form.get('email')?.hasError('emailTaken')) { This email is already registered }
  </mat-error>
</mat-form-field>
```

### Page-Level Error States

```
┌───────────────────────────────────────────────────┐
│              [Error Illustration]                   │
│                                                     │
│         Something went wrong                        │
│   We couldn't load the internships. Please try      │
│   again or contact support if the issue persists.   │
│                                                     │
│     [Try Again]     [Go to Home]                    │
└───────────────────────────────────────────────────┘
```

**Error Types**:
1. **Network error** (offline): "No internet connection" + retry.
2. **Server error** (5xx): "Something went wrong" + retry + support link.
3. **Not found** (404): Custom 404 page with search CTA.
4. **Forbidden** (403): "Access denied" + sign-in CTA.
5. **Session expired** (401): Toast notification + auto-redirect to login.
