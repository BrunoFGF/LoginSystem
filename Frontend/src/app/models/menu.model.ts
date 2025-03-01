export interface MenuItem {
  id: number;
  name: string;
  icon: string;
  url: string;
  parentId?: number;
  roles: string[];
}
