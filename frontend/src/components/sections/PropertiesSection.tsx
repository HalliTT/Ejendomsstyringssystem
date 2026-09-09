import { useQuery } from "@tanstack/react-query";
import { getProperties } from "@/api/properties";

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
    <div className="properties-section">
      <h2>Properties Section</h2>
      {properties?.map((property) => (
        <div key={property.id}>
          <h2>{property.address}</h2>
          <p>{property.city}</p>
          <p>{property.monthlyRent} kr.</p>
        </div>
      ))}
    </div>
  );
}
