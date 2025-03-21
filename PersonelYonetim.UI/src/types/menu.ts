/**
 * Dashboard düzeninde kullanılan menü öğeleri için arayüz
 */
export interface MenuItem {
  name: string;
  icon: string;
  active: boolean;
  path: string;
  roles?: string[];
}
