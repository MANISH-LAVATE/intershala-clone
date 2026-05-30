export type CourseLevel = 'Beginner' | 'Intermediate' | 'Advanced';

export interface CourseListItem {
  id: number;
  title: string;
  instructor: string | null;
  thumbnailUrl: string | null;
  level: CourseLevel;
  categoryName: string;
  durationHours: number;
  isFree: boolean;
  price: number;
  enrolledCount: number;
  moduleCount: number;
  createdAt: string;
}

export interface CourseModule {
  id: number;
  title: string;
  description: string | null;
  videoUrl: string | null;
  orderIndex: number;
  durationMinutes: number;
  isPreview: boolean;
}

export interface CourseDetail extends CourseListItem {
  description: string;
  categoryId: number;
  language: string | null;
  prerequisites: string | null;
  whatYouLearn: string | null;
  modules: CourseModule[];
  isEnrolled: boolean;
  enrollmentProgress: number;
}

export interface Enrollment {
  id: number;
  courseId: number;
  courseTitle: string;
  courseThumbnailUrl: string | null;
  progressPercent: number;
  completedModules: number;
  totalModules: number;
  enrolledAt: string;
  completedAt: string | null;
  lastAccessedAt: string | null;
}

export interface CourseFilters {
  search: string;
  categoryId: number | null;
  level: CourseLevel | null;
  isFree: boolean | null;
  page: number;
  pageSize: number;
  sortBy: string;
}
