from PIL import Image
import os

# Configure paths
input_folder = "C:/Users/RoboH/unity_meta_quest_ros/gaussian_splatting_setup/splat_input/input"  
output_folder = "C:/Users/RoboH/unity_meta_quest_ros/gaussian_splatting_setup/splat_input/temp"
crop_pixels = 40  # Set how many pixels to crop from the top

def crop_images_in_folder():
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    for filename in os.listdir(input_folder):
        if filename.lower().endswith(('.png', '.jpg', '.jpeg')):  # Process only images
            input_path = os.path.join(input_folder, filename)
            output_path = os.path.join(output_folder, filename)

            img = Image.open(input_path)
            width, height = img.size

            if crop_pixels >= height:
                print(f"Skipping {filename}: Crop size too large!")
                continue

            cropped_img = img.crop((0, crop_pixels, width, height))
            cropped_img.save(output_path)
            print(f"Cropped: {filename}")

if __name__ == "__main__":
    crop_images_in_folder()
    print("Batch processing complete!")