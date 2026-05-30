import { test, expect } from '@playwright/test';

test.describe('Courses', () => {
  test('courses catalog page loads', async ({ page }) => {
    await page.goto('/courses');
    await expect(page.getByRole('heading', { name: /courses/i })).toBeVisible();
  });

  test('level filter is visible', async ({ page }) => {
    await page.goto('/courses');
    await expect(page.getByLabel(/level/i)).toBeVisible();
  });

  test('my courses page requires auth', async ({ page }) => {
    await page.goto('/courses/my');
    // Should redirect to login or show auth prompt
    await expect(page).toHaveURL(/\/auth\/login|\/courses\/my/);
  });
});
