import { test, expect } from '@playwright/test';

const uniqueEmail = () => `test_${Date.now()}@e2e.internshala.test`;

test.describe('Authentication', () => {
  test('home page redirects unauthenticated user to internships', async ({ page }) => {
    await page.goto('/');
    await expect(page).toHaveURL(/\/internships/);
  });

  test('login page is accessible', async ({ page }) => {
    await page.goto('/auth/login');
    await expect(page.getByRole('heading', { name: /login/i })).toBeVisible();
  });

  test('register page is accessible', async ({ page }) => {
    await page.goto('/auth/register');
    await expect(page.getByRole('heading', { name: /register|sign up/i })).toBeVisible();
  });

  test('login form shows validation errors on empty submit', async ({ page }) => {
    await page.goto('/auth/login');
    await page.getByRole('button', { name: /login|sign in/i }).click();

    // At least one error should appear (email required, password required)
    await expect(page.locator('mat-error').first()).toBeVisible();
  });

  test('login with wrong credentials shows error message', async ({ page }) => {
    await page.goto('/auth/login');

    await page.getByLabel(/email/i).fill('nobody@example.com');
    await page.getByLabel(/password/i).fill('WrongPassword123!');
    await page.getByRole('button', { name: /login|sign in/i }).click();

    // Should show an error snackbar or inline error
    await expect(
      page.locator('mat-snack-bar-container, [role="alert"]').first(),
    ).toBeVisible({ timeout: 8000 });
  });

  test('register as student shows form fields', async ({ page }) => {
    await page.goto('/auth/register');

    // Verify key form fields exist
    await expect(page.getByLabel(/email/i)).toBeVisible();
    await expect(page.getByLabel(/password/i).first()).toBeVisible();
    await expect(page.getByLabel(/first name/i)).toBeVisible();
    await expect(page.getByLabel(/last name/i)).toBeVisible();
  });

  test('authenticated student can reach My Applications page', async ({ page }) => {
    // This test requires a live backend; skip if not running
    test.skip(
      !process.env['E2E_STUDENT_EMAIL'],
      'Requires E2E_STUDENT_EMAIL env variable',
    );

    await page.goto('/auth/login');
    await page.getByLabel(/email/i).fill(process.env['E2E_STUDENT_EMAIL']!);
    await page.getByLabel(/password/i).fill(process.env['E2E_STUDENT_PASSWORD']!);
    await page.getByRole('button', { name: /login|sign in/i }).click();

    await page.goto('/applications');
    await expect(page.getByRole('heading', { name: /my applications/i })).toBeVisible();
  });
});
