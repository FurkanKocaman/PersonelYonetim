import axios from 'axios';

// Define interfaces for the data structures
export interface StatItem {
  title: string;
  value: number;
  icon: string;
  color: string;
}

export interface PayrollItem {
  id: number;
  name: string;
  department: string;
  status: string;
  date: string;
}

export interface Announcement {
  id: number;
  title: string;
  date: string;
  content: string;
}

export interface QuickAccessButton {
  id: number;
  title: string;
  icon: string;
  color: string;
  action?: string; // API endpoint or route to call when clicked
}

class DashboardService {
  private apiUrl = 'https://localhost:7063/api';

  // Get dashboard statistics
  async getStatistics() {
    try {
      const response = await axios.get(`${this.apiUrl}/dashboard/statistics`);
      return response.data.data as StatItem[];
    } catch (error) {
      console.error('Error fetching statistics:', error);
      throw error;
    }
  }

  // Get payroll items
  async getPayrollItems() {
    try {
      const response = await axios.get(`${this.apiUrl}/dashboard/payroll`);
      return response.data.data as PayrollItem[];
    } catch (error) {
      console.error('Error fetching payroll items:', error);
      throw error;
    }
  }

  // Get announcements
  async getAnnouncements() {
    try {
      const response = await axios.get(`${this.apiUrl}/dashboard/announcements`);
      return response.data.data as Announcement[];
    } catch (error) {
      console.error('Error fetching announcements:', error);
      throw error;
    }
  }

  // Get quick access buttons
  async getQuickAccessButtons() {
    try {
      const response = await axios.get(`${this.apiUrl}/dashboard/quick-access`);
      return response.data.data as QuickAccessButton[];
    } catch (error) {
      console.error('Error fetching quick access buttons:', error);
      throw error;
    }
  }
}

export default new DashboardService();
