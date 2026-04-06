import AppModeSwitcher from "./AppModeSwitcher";
import UserMenu from "./UserMenu";

type SearchField = {
  name: string;
  value: string;
};

type UtilityPlaceholder = {
  title: string;
};

type SharedTopBarProps = {
  section: string;
  title: string;
  subtitle?: string;
  search?: {
    action: string;
    placeholder: string;
    defaultValue?: string;
    queryName?: string;
    hiddenFields?: SearchField[];
  };
  utilities?: {
    left?: UtilityPlaceholder;
    right?: UtilityPlaceholder;
  };
};

function SearchIcon() {
  return (
    <svg aria-hidden="true" className="h-4 w-4" fill="none" viewBox="0 0 24 24">
      <circle cx="11" cy="11" r="6.5" stroke="currentColor" strokeWidth="1.7" />
      <path d="M16 16l4.5 4.5" stroke="currentColor" strokeLinecap="round" strokeWidth="1.7" />
    </svg>
  );
}

export default async function SharedTopBar({
  section,
  title,
  subtitle,
  search,
  utilities
}: SharedTopBarProps) {
  const queryName = search?.queryName ?? "name";
  const hasUtilities = Boolean(utilities?.left || utilities?.right);

  return (
    <div className="space-y-4">
      <div className="flex w-full flex-col gap-4 lg:flex-row lg:items-start lg:justify-between">
        {search ? (
          <div className="min-w-0 max-w-xl flex-1 space-y-2 lg:max-w-md xl:max-w-xl">
            <form
              action={search.action}
              className="flex min-w-0 items-center gap-3 rounded-[1.4rem] border border-stone-200 bg-stone-50/90 px-3 py-3 shadow-[0_10px_24px_rgba(120,92,60,0.08)]"
              role="search"
            >
              <SearchIcon />
              <input
                aria-label={search.placeholder}
                className="min-w-0 flex-1 bg-transparent text-sm text-stone-900 outline-none placeholder:text-stone-400"
                defaultValue={search.defaultValue}
                name={queryName}
                placeholder={search.placeholder}
                type="search"
              />
              {search.hiddenFields?.map((field) => (
                <input key={field.name} name={field.name} type="hidden" value={field.value} />
              ))}
              <button
                className="inline-flex h-10 w-10 items-center justify-center rounded-2xl bg-stone-950 text-white transition hover:bg-orange-500"
                type="submit"
              >
                <SearchIcon />
              </button>
            </form>
            {subtitle ? (
              <div className="hidden pl-1 text-xs uppercase tracking-[0.26em] text-stone-400 lg:block">
                {subtitle}
              </div>
            ) : null}
          </div>
        ) : null}

        <div className="min-w-0 lg:ml-auto">
          <div className="flex flex-wrap items-center justify-end gap-2 text-sm font-medium text-stone-600">
            <UserMenu />
            <AppModeSwitcher />
          </div>
        </div>
      </div>

      {hasUtilities ? (
        <div className="flex flex-col gap-3 lg:flex-row lg:items-center lg:justify-between">
          {utilities?.left ? (
            <div className="flex rounded-[1.15rem] border border-stone-200 bg-white px-4 py-3 text-sm font-semibold tracking-tight text-stone-950 shadow-[0_8px_24px_rgba(120,92,60,0.08)]">
              {utilities.left.title}
            </div>
          ) : (
            <div className="hidden lg:block" />
          )}

          {utilities?.right ? (
            <div className="flex rounded-[1.15rem] border border-stone-200 bg-white px-4 py-3 text-sm font-semibold tracking-tight text-stone-950 shadow-[0_8px_24px_rgba(120,92,60,0.08)] lg:justify-end">
              {utilities.right.title}
            </div>
          ) : null}
        </div>
      ) : null}
    </div>
  );
}