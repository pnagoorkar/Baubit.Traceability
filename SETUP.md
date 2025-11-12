# Setup Guide for Baubit.Traceability

This document provides step-by-step instructions for completing the CI/CD setup for the Baubit.Traceability repository.

## Prerequisites

- Repository access to pnagoorkar/Baubit.Traceability on GitHub
- Access to CircleCI organization
- Access to Codecov.io
- Access to Snyk.io (optional)

## 1. CircleCI Configuration

### 1.1. Connect Repository to CircleCI

1. Go to [CircleCI](https://circleci.com/)
2. Navigate to Projects
3. Find `pnagoorkar/Baubit.Traceability` and click "Set Up Project"
4. Select "Use existing config" since `.circleci/config.yml` is already in the repository
5. Click "Start Building"

### 1.2. Configure Context Variables

The CircleCI configuration uses a context named `Context_Prashant`. Add the following environment variables:

1. Navigate to Organization Settings → Contexts
2. Select or create `Context_Prashant`
3. Add the following environment variables:

#### Required Variables:

- **`CODECOV_TOKEN_Baubit_Traceability`**
  - Value: Get from Codecov.io after setting up the project (see section 2)
  - Description: Token for uploading code coverage to Codecov

- **`DOCKER_HUB_USERNAME`**
  - Value: Docker Hub username
  - Description: Used for authenticating with Docker Hub to pull build images

- **`DOCKER_HUB_PASSWORD`**
  - Value: Docker Hub password or access token
  - Description: Used for authenticating with Docker Hub

- **`GITHUB_USERNAME`**
  - Value: GitHub username (pnagoorkar)
  - Description: Used for publishing packages to GitHub Packages

- **`GITHUB_TOKEN`**
  - Value: GitHub Personal Access Token with `write:packages` scope
  - Description: Used for authenticating with GitHub Packages
  - To create: GitHub Settings → Developer settings → Personal access tokens → Generate new token

- **`NUGET_API_KEY`** (for release branch only)
  - Value: NuGet.org API key
  - Description: Used for publishing packages to NuGet.org
  - To create: NuGet.org → Account Settings → API Keys → Create

## 2. Codecov.io Integration

### 2.1. Import Repository

1. Go to [Codecov.io](https://codecov.io)
2. Sign in with GitHub
3. Navigate to "+ Add new repository"
4. Find and select `pnagoorkar/Baubit.Traceability`
5. Copy the repository upload token

### 2.2. Configure Codecov

1. Add the copied token to CircleCI context as `CODECOV_TOKEN_Baubit_Traceability` (see section 1.2)
2. The `codecov.yml` file in the repository root is already configured with:
   - 100% coverage target
   - Proper ignore patterns for test files
   - Build artifact exclusions

### 2.3. Add Badge to README (Optional)

The README already includes a Codecov badge. Once the first build completes, it will start showing actual coverage.

## 3. Snyk.io Integration (Optional Security Scanning)

### 3.1. Import Repository

1. Go to [Snyk.io](https://snyk.io)
2. Sign in with GitHub
3. Click "Add project"
4. Select `pnagoorkar/Baubit.Traceability`
5. Configure scanning preferences (recommended: scan on every PR and commit)

### 3.2. Configure Webhooks

Snyk should automatically configure webhooks. Verify:
1. Go to repository settings on GitHub
2. Navigate to Webhooks
3. Verify Snyk webhook is present and active

## 4. GitHub Repository Settings

### 4.1. Branch Protection Rules

#### Master Branch
1. Go to repository Settings → Branches
2. Add rule for `master` branch:
   - ✅ Require a pull request before merging
   - ✅ Require approvals (minimum 1)
   - ✅ Require status checks to pass before merging
     - Select: `ci/circleci: build`
     - Select: `ci/circleci: test`
   - ✅ Require branches to be up to date before merging
   - ✅ Do not allow bypassing the above settings

#### Release Branch
1. Add rule for `release` branch:
   - ✅ Require a pull request before merging
   - ✅ Require approvals (minimum 1)
   - ✅ Require status checks to pass before merging
     - Select: `ci/circleci: build`
     - Select: `ci/circleci: test`
   - ✅ Require branches to be up to date before merging
   - ✅ Do not allow bypassing the above settings

### 4.2. GitHub Packages Configuration

1. Navigate to repository Settings → Packages
2. Verify package visibility settings
3. Optionally link packages to the repository

### 4.3. Security Settings

1. Navigate to repository Settings → Security & analysis
2. Enable:
   - ✅ Dependency graph
   - ✅ Dependabot alerts
   - ✅ Dependabot security updates
   - ✅ Secret scanning
   - ✅ Code scanning (if available)

## 5. Initial Build Verification

After completing the setup:

1. Trigger a build by pushing a commit or creating a PR
2. Monitor the build in CircleCI
3. Verify all jobs complete successfully:
   - ✅ build
   - ✅ test
   - ✅ coverage upload to Codecov
4. Check Codecov dashboard for coverage report
5. Verify Snyk scan results (if configured)

## 6. Publishing Configuration

### 6.1. NuGet Package Metadata

The package metadata is already configured in `Baubit.Traceability.csproj`:
- Package ID: `Baubit.Traceability`
- Authors: `Prashant Nagoorkar`
- Description, tags, and repository URL are set
- README and LICENSE are included in the package

### 6.2. Version Management

Version numbers can be managed through:
- Git tags (recommended)
- Manual version updates in `.csproj`
- Automatic versioning based on commit messages

## 7. Troubleshooting

### Build Fails on CircleCI
- Check that all context variables are correctly set
- Verify Docker Hub credentials are valid
- Check CircleCI logs for specific error messages

### Coverage Upload Fails
- Verify `CODECOV_TOKEN_Baubit_Traceability` is set correctly
- Check that coverage file is being generated in `TestResults/`
- Verify Codecov repository token matches

### Package Publishing Fails
- Verify GitHub token has correct permissions (`write:packages`)
- Check NuGet API key is valid (for NuGet.org publishing)
- Ensure package version doesn't already exist

## 8. Maintenance Checklist

- [ ] Review and approve Dependabot PRs regularly
- [ ] Monitor Snyk security alerts
- [ ] Keep CircleCI context variables updated
- [ ] Rotate tokens and credentials periodically
- [ ] Review and update branch protection rules as needed
- [ ] Monitor code coverage trends in Codecov

## Additional Resources

- [CircleCI Documentation](https://circleci.com/docs/)
- [Codecov Documentation](https://docs.codecov.com/)
- [Snyk Documentation](https://docs.snyk.io/)
- [NuGet Package Publishing](https://docs.microsoft.com/nuget/nuget-org/publish-a-package)
- [GitHub Packages](https://docs.github.com/packages)

---

**Last Updated**: November 2025
