export type ApplicationStatus =
  | 'Applied'
  | 'UnderReview'
  | 'Shortlisted'
  | 'InterviewScheduled'
  | 'Selected'
  | 'Rejected'
  | 'Withdrawn';

export const APPLICATION_STATUS_LABELS: Record<ApplicationStatus, string> = {
  Applied: 'Applied',
  UnderReview: 'Under Review',
  Shortlisted: 'Shortlisted',
  InterviewScheduled: 'Interview Scheduled',
  Selected: 'Selected',
  Rejected: 'Rejected',
  Withdrawn: 'Withdrawn',
};

export const APPLICATION_STATUS_COLORS: Record<ApplicationStatus, string> = {
  Applied: 'primary',
  UnderReview: 'accent',
  Shortlisted: 'accent',
  InterviewScheduled: 'accent',
  Selected: 'primary',
  Rejected: 'warn',
  Withdrawn: '',
};

export interface ApplicationListItem {
  id: number;
  listingType: 'Internship' | 'Job';
  internshipId: number | null;
  internshipTitle: string | null;
  jobId: number | null;
  jobTitle: string | null;
  companyName: string;
  companyLogoUrl: string | null;
  status: ApplicationStatus;
  coverLetter: string | null;
  resumeUrl: string;
  availabilityDate: string | null;
  expectedStipend: number | null;
  employerNote: string | null;
  rejectionReason: string | null;
  appliedAt: string;
  withdrawnAt: string | null;
}

export interface StatusHistoryEntry {
  fromStatus: ApplicationStatus;
  toStatus: ApplicationStatus;
  comment: string | null;
  changedAt: string;
}

export interface ApplicationDetail extends ApplicationListItem {
  internshipDescription: string | null;
  jobDescription: string | null;
  employerId: number;
  statusHistory: StatusHistoryEntry[];
}

export interface EmployerApplicationItem {
  id: number;
  studentId: number;
  studentFullName: string;
  studentEmail: string | null;
  studentInstitution: string | null;
  studentCourse: string | null;
  graduationYear: number | null;
  status: ApplicationStatus;
  coverLetter: string | null;
  resumeUrl: string;
  availabilityDate: string | null;
  expectedStipend: number | null;
  employerNote: string | null;
  appliedAt: string;
}

export interface ApplyRequest {
  listingType: 'Internship' | 'Job';
  listingId: number;
  coverLetter: string | null;
  resumeUrl: string;
  availabilityDate: string | null;
  expectedStipend: number | null;
}

export interface UpdateStatusRequest {
  newStatus: ApplicationStatus;
  note: string | null;
}

export interface ApplicationFilters {
  status: ApplicationStatus | null;
  listingType: 'Internship' | 'Job' | null;
  page: number;
  pageSize: number;
}
