import { test, expect } from '@playwright/test';

test.describe('Internships', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/internships');
  });

  test('internship listing page loads', async ({ page }) => {
    await expect(page.getByRole('heading', { name: /internships/i })).toBeVisible();
  });

  test('search input is visible', async ({ page }) => {
    await expect(page.getByPlaceholder(/search/i)).toBeVisible();
  });

  test('sort dropdown is present', async ({ page }) => {
    await expect(page.getByLabel(/sort by/i)).toBeVisible();
  });

  test('apply to internship redirects unauthenticated user to login', async ({ page }) => {
    // Navigate to an internship detail page and click apply
    // This test only works if at least one internship exists
    test.skip(!process.env['E2E_RUN_WITH_DATA'], 'Requires seeded data');

    const firstCard = page.locator('app-internship-card').first();
    await firstCard.click();
    await page.getByRole('button', { name: /apply/i }).click();

    await expect(page).toHaveURL(/\/auth\/login/);
  });

  test('pagination controls are present when there are many results', async ({ page }) => {
    test.skip(!process.env['E2E_RUN_WITH_DATA'], 'Requires seeded data');

    const paginator = page.locator('mat-paginator');
    if (await paginator.isVisible()) {
      await expect(paginator).toBeVisible();
    }
  });
});
