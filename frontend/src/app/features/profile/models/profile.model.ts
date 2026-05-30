export interface StudentProfile {
  id: number;
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  profilePictureUrl: string | null;
  dateOfBirth: string | null;
  gender: string | null;
  currentInstitution: string | null;
  courseOfStudy: string | null;
  graduationYear: number | null;
  gpa: number | null;
  bio: string | null;
  linkedInUrl: string | null;
  gitHubUrl: string | null;
  portfolioUrl: string | null;
  profileCompleteness: number;
  isProfileComplete: boolean;
  educations: Education[];
  experiences: Experience[];
  resume: Resume | null;
}

export interface Education {
  id: number;
  institution: string;
  degree: string;
  fieldOfStudy: string;
  startYear: number;
  endYear: number | null;
  isCurrent: boolean;
  grade: number | null;
}

export interface Experience {
  id: number;
  title: string;
  company: string;
  description: string | null;
  startDate: string;
  endDate: string | null;
  isCurrent: boolean;
}

export interface Resume {
  id: number;
  fileUrl: string | null;
  fileName: string | null;
  fileSizeBytes: number | null;
  uploadedAt: string | null;
}

export interface UpdateProfileForm {
  firstName: string;
  lastName: string;
  phoneNumber: string | null;
  dateOfBirth: string | null;
  gender: string | null;
  currentInstitution: string | null;
  courseOfStudy: string | null;
  graduationYear: number | null;
  gpa: number | null;
  bio: string | null;
  linkedInUrl: string | null;
  gitHubUrl: string | null;
  portfolioUrl: string | null;
}

export interface AddEducationForm {
  institution: string;
  degree: string;
  fieldOfStudy: string;
  startYear: number;
  endYear: number | null;
  isCurrent: boolean;
  grade: number | null;
}

export interface AddExperienceForm {
  title: string;
  company: string;
  description: string | null;
  startDate: string;
  endDate: string | null;
  isCurrent: boolean;
}
