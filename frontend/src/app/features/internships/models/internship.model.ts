export type InternshipType = 'InOffice' | 'Remote' | 'Hybrid';

export interface InternshipListItem {
  id: number;
  title: string;
  companyName: string;
  companyLogoUrl: string | null;
  isCompanyVerified: boolean;
  location: string | null;
  internshipType: InternshipType;
  stipendMin: number | null;
  stipendMax: number | null;
  isPaid: boolean;
  durationMonths: number;
  applicationDeadline: string | null;
  applicationsCount: number;
  isFeatured: boolean;
  createdAt: string;
  skills: string[];
}

export interface InternshipDetail extends InternshipListItem {
  description: string;
  responsibilities: string | null;
  requirements: string | null;
  employerId: number;
  companyWebsite: string | null;
  isCompanyVerified: boolean;
  companyDescription: string | null;
  categoryId: number;
  categoryName: string;
  startDateType: string;
  startDate: string | null;
  openingsCount: number;
  viewsCount: number;
  publishedAt: string;
}

export interface InternshipFilters {
  search: string;
  categoryId: number | null;
  locationId: number | null;
  stipendMin: number | null;
  isRemote: boolean;
  internshipType: InternshipType | null;
  durationMonths: number | null;
  page: number;
  pageSize: number;
  sortBy: string;
  sortOrder: 'asc' | 'desc';
}

export const defaultFilters: InternshipFilters = {
  search: '',
  categoryId: null,
  locationId: null,
  stipendMin: null,
  isRemote: false,
  internshipType: null,
  durationMonths: null,
  page: 1,
  pageSize: 20,
  sortBy: 'createdAt',
  sortOrder: 'desc',
};

export interface CreateInternshipForm {
  title: string;
  description: string;
  responsibilities: string | null;
  requirements: string | null;
  categoryId: number;
  locationId: number | null;
  internshipType: InternshipType;
  stipendMin: number | null;
  stipendMax: number | null;
  isPaid: boolean;
  durationMonths: number;
  startDateType: string;
  startDate: string | null;
  openingsCount: number;
  applicationDeadline: string | null;
  skillIds: number[];
}
