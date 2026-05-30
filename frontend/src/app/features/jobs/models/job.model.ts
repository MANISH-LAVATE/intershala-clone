export type JobType = 'InOffice' | 'Remote' | 'Hybrid';

export interface JobListItem {
  id: number;
  title: string;
  companyName: string;
  companyLogoUrl: string | null;
  isCompanyVerified: boolean;
  location: string | null;
  jobType: JobType;
  salaryMin: number | null;
  salaryMax: number | null;
  experienceYearsMin: number | null;
  applicationDeadline: string | null;
  applicationsCount: number;
  createdAt: string;
  skills: string[];
}

export interface JobDetail extends JobListItem {
  description: string;
  requirements: string | null;
  employerId: number;
  categoryId: number;
  categoryName: string;
  publishedAt: string;
}

export interface JobFilters {
  search: string;
  categoryId: number | null;
  locationId: number | null;
  salaryMin: number | null;
  isRemote: boolean;
  page: number;
  pageSize: number;
}

export const defaultJobFilters: JobFilters = {
  search: '',
  categoryId: null,
  locationId: null,
  salaryMin: null,
  isRemote: false,
  page: 1,
  pageSize: 20,
};

export interface CreateJobForm {
  title: string;
  description: string;
  requirements: string | null;
  categoryId: number;
  locationId: number | null;
  jobType: JobType;
  salaryMin: number | null;
  salaryMax: number | null;
  experienceYearsMin: number | null;
  applicationDeadline: string | null;
  skillIds: number[];
}
