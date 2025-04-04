import api from "./Axios";

class FileService {
  async uploadProfileImage(file: File): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("file", file);
      const response = await api.post(`/files/upload-profile-image`, formData);
      return response.data.data;
    } catch (error) {
      console.error("Error uploading file", error);
      throw error;
    }
  }
}

export default new FileService();
