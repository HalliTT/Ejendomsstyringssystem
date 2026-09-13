import { useQuery } from "@tanstack/react-query";
import "@/components/sections/PropertiesSection.css";
import { getProperties } from "@/api/properties";
import { PropertyCard } from "@/components/sections/PropertyCard";

export function PropertiesSection() {
  const {
    data: properties,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["properties"],
    queryFn: getProperties,
  });

  if (isLoading) {
    return <p>Loading properties...</p>;
  }

  if (error) {
    return <p>Error loading properties: {error.message}</p>;
  }

  return (
    <div>
      <div className="properties-grid">
        {properties?.map((property) => (
          <PropertyCard key={property.id} property={property}></PropertyCard>
        ))}
      </div>
    </div>
  );
}
