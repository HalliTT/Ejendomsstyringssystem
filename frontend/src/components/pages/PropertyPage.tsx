import { useParams } from "react-router";
import { useQuery } from "@tanstack/react-query";
import { getProperty } from "@/api/properties";

export function PropertyPage() {
  const { propertyId } = useParams();

  const {
    data: property,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["property", propertyId],
    queryFn: () => getProperty(propertyId!),
  });

  if (isLoading) {
    return <p>Loading properties...</p>;
  }

  if (error) {
    return <p>Error loading properties: {error.message}</p>;
  }

  return (
    <div>
      <p>Property ID: {propertyId}</p>

      <h1>Property</h1>

      <h2>Units</h2>
    </div>
  );
}
