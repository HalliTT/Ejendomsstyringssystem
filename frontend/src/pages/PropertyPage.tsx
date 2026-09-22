import "@/pages/PropertyPage.css";
import { useParams } from "react-router";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { getProperty, updateProperty, deleteProperty } from "@/api/properties";
import { updateUnit, deleteUnit, createUnit } from "@/api/units";
import {
  createRentalOption,
  updateRentalOption,
  deleteRentalOption,
} from "@/api/rentalOptions";
import type {
  PropertyInput,
  RentalOption,
  RentalOptionFormInput,
  Unit,
  UnitInput,
} from "@/types";
import { useNavigate } from "react-router";

import {
  ArrowLeftIcon,
  MapPinIcon,
  PencilIcon,
  PlusIcon,
  TrashIcon,
} from "@/components/ui/Icons";
import { Card, CardHeader } from "@/components/ui/Card";
import { UnitsTable } from "@/components/dashboard/UnitsTable";
import { useState } from "react";
import { ConfirmDialog } from "@/components/ui/ConfirmDialog";
import { PropertyModal } from "@/components/dashboard/PropertyModal";
import { useModalState } from "@/hooks/useModalState";
import { UnitModal } from "@/components/dashboard/UnitModal";
import { RentalOptionsList } from "@/components/dashboard/RentalOptionsList";
import { RentalOptionModal } from "@/components/dashboard/RentalOptionModal";

export function PropertyPage() {
  const { propertyId } = useParams();
  const [isConfirmOpen, setConfirmOpen] = useState(false);
  const updatedPropertyModal = useModalState(false);
  const unitModal = useModalState(false);
  const navigate = useNavigate();

  const [editingUnit, setEditingUnit] = useState<Unit | null>(null);
  const [unitPendingDelete, setUnitPendingDelete] = useState<Unit | null>(null);

  const rentalOptionModal = useModalState(false);
  const [editingRentalOption, setEditingRentalOption] =
    useState<RentalOption | null>(null);
  const [rentalOptionPendingDelete, setRentalOptionPendingDelete] =
    useState<RentalOption | null>(null);

  const queryClient = useQueryClient();

  const {
    data: property,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["property", propertyId],
    queryFn: () => getProperty(propertyId!),
    enabled: !!propertyId,
  });

  const updatePropertyMutation = useMutation({
    mutationFn: (data: PropertyInput) => updateProperty(data, propertyId!),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["property", propertyId],
      });

      queryClient.invalidateQueries({
        queryKey: ["properties"],
      });
    },
  });

  const deletePropertyMutation = useMutation({
    mutationFn: deleteProperty,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["properties"],
      });
      setConfirmOpen(false);
      navigate("/dashboard/properties");
    },
  });

  const createUnitMutation = useMutation({
    mutationFn: (data: UnitInput) => createUnit(data, propertyId!),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["property", propertyId],
      });

      queryClient.invalidateQueries({
        queryKey: ["properties"],
      });
    },
  });

  const updateUnitMutation = useMutation({
    mutationFn: (data: UnitInput) => updateUnit(data, editingUnit?.id!),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["property", propertyId],
      });

      queryClient.invalidateQueries({
        queryKey: ["properties"],
      });
    },
  });

  const deleteUnitMutation = useMutation({
    mutationFn: deleteUnit,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["property", propertyId],
      });

      queryClient.invalidateQueries({
        queryKey: ["properties"],
      });
    },
  });

  const createRentalOptionMutation = useMutation({
    mutationFn: (data: RentalOptionFormInput) =>
      createRentalOption(data, propertyId!),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["property", propertyId],
      });
    },
  });

  const updateRentalOptionMutation = useMutation({
    mutationFn: (data: RentalOptionFormInput) =>
      updateRentalOption(data, editingRentalOption?.id!),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["property", propertyId],
      });
    },
  });

  const deleteRentalOptionMutation = useMutation({
    mutationFn: deleteRentalOption,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["property", propertyId],
      });
    },
  });

  // PROPERTY

  const handleConfirmDelete = () => {
    if (!propertyId) return;
    deletePropertyMutation.mutate(propertyId);
  };

  const submitProperty = async (propertyData: PropertyInput) => {
    await updatePropertyMutation.mutateAsync(propertyData);
  };

  // UNIT

  const openAddUnit = () => {
    setEditingUnit(null);
    unitModal.open();
  };

  const openEditUnit = (unit: Unit) => {
    setEditingUnit(unit);
    unitModal.open();
  };

  const handleConfirmDeleteUnit = () => {
    if (!unitPendingDelete) return;
    deleteUnitMutation.mutate(unitPendingDelete.id);
    setUnitPendingDelete(null);
  };

  const submitUnit = async (unitData: UnitInput) => {
    if (editingUnit) {
      await updateUnitMutation.mutateAsync(unitData);
    } else {
      await createUnitMutation.mutateAsync(unitData);
    }
  };

  // RENTAL OPTION

  const openAddRentalOption = () => {
    setEditingRentalOption(null);
    rentalOptionModal.open();
  };

  const openEditRentalOption = (option: RentalOption) => {
    setEditingRentalOption(option);
    rentalOptionModal.open();
  };

  const handleConfirmDeleteRentalOption = () => {
    if (!rentalOptionPendingDelete) return;
    deleteRentalOptionMutation.mutate(rentalOptionPendingDelete.id);
    setRentalOptionPendingDelete(null);
  };

  const submitRentalOption = async (input: RentalOptionFormInput) => {
    if (editingRentalOption) {
      await updateRentalOptionMutation.mutateAsync(input);
    } else {
      await createRentalOptionMutation.mutateAsync(input);
    }
  };

  if (isLoading) {
    return <p>Loading properties...</p>;
  }

  if (error) {
    return <p>Error loading properties: {error.message}</p>;
  }

  return (
    <div>
      <button
        type="button"
        className="detail-back-btn"
        onClick={() => navigate("/dashboard/properties")}
      >
        <ArrowLeftIcon width={16} height={16} />
        Back to properties
      </button>

      <div
        className="detail-hero"
        style={{
          background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
        }}
      >
        <div className="detail-hero-overlay" />
        <div className="detail-hero-content">
          <div>
            <div className="detail-hero-name">{property?.name}</div>
            <div className="detail-hero-address">
              <MapPinIcon width={14} height={14} />
              {property?.address}, {property?.city}
            </div>
          </div>

          <div className="detail-hero-meta">
            <button
              type="button"
              className="detail-hero-btn"
              onClick={updatedPropertyModal.open}
            >
              <PencilIcon width={15} height={15} />
              Edit
            </button>
            <button
              type="button"
              className="detail-hero-btn detail-hero-btn-danger"
              onClick={() => setConfirmOpen(true)}
            >
              <TrashIcon width={15} height={15} />
              Delete
            </button>
          </div>
        </div>
      </div>

      <div className="detail-stats-grid">
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{property?.totalUnits}</div>
          <div className="detail-stat-label">Total units</div>
        </Card>
        <Card className="detail-stat-card card-padded">
          <div className="detail-stat-value">{property?.occupiedUnits}</div>
          <div className="detail-stat-label">Occupied</div>
        </Card>
      </div>

      <div className="detail-section">
        <div className="detail-section-two-col">
          <Card className="card-padded">
            <CardHeader
              title="Units"
              subtitle={`${property?.totalUnits} total · ${property?.occupiedUnits} occupied`}
              action={
                <button
                  type="button"
                  className="detail-section-add-btn"
                  onClick={openAddUnit}
                >
                  <PlusIcon width={15} height={15} />
                  Add unit
                </button>
              }
            />
            <UnitsTable
              units={property?.units}
              onEdit={openEditUnit}
              onDelete={setUnitPendingDelete}
            />
          </Card>
          <Card className="card-padded">
            <CardHeader
              title="Rental options"
              subtitle={"Rent a single unit, or a bundle of several"}
              action={
                <button
                  type="button"
                  className="detail-section-add-btn"
                  onClick={openAddRentalOption}
                >
                  <PlusIcon width={15} height={15} />
                  Add rental option
                </button>
              }
            />
            <RentalOptionsList
              rentalOptions={property?.rentalOptions ?? []}
              units={property?.units}
              onEdit={openEditRentalOption}
              onDelete={setRentalOptionPendingDelete}
            />
          </Card>
        </div>
      </div>
      <PropertyModal
        isOpen={updatedPropertyModal.isOpen}
        onClose={() => {
          updatedPropertyModal.close();
          updatePropertyMutation.reset();
        }}
        onSubmit={submitProperty}
        initialValues={property}
        isSubmitting={updatePropertyMutation.isPending}
        isSuccess={updatePropertyMutation.isSuccess}
      />
      <ConfirmDialog
        isOpen={isConfirmOpen}
        title="Delete"
        message={`Are you sure you want to delete, ${property?.name}?`}
        confirmLabel="Delete"
        onConfirm={handleConfirmDelete}
        onCancel={() => setConfirmOpen(false)}
      />

      <UnitModal
        isOpen={unitModal.isOpen}
        onClose={() => {
          unitModal.close();
          createUnitMutation.reset();
          updateUnitMutation.reset();
        }}
        onSubmit={submitUnit}
        initialValues={
          editingUnit
            ? {
                name: editingUnit.name,
                description: editingUnit.description,
                status: editingUnit.status,
              }
            : undefined
        }
        isSubmitting={
          editingUnit
            ? updateUnitMutation.isPending
            : createUnitMutation.isPending
        }
        isSuccess={
          editingUnit
            ? updateUnitMutation.isSuccess
            : createUnitMutation.isSuccess
        }
      />
      <ConfirmDialog
        isOpen={!!unitPendingDelete}
        title="Delete unit"
        message={`Are you sure you want to delete unit, ${unitPendingDelete?.name}?`}
        confirmLabel="Delete unit"
        onConfirm={handleConfirmDeleteUnit}
        onCancel={() => setUnitPendingDelete(null)}
      />

      <RentalOptionModal
        isOpen={rentalOptionModal.isOpen}
        onClose={() => {
          rentalOptionModal.close();
          createRentalOptionMutation.reset();
          updateRentalOptionMutation.reset();
        }}
        onSubmit={submitRentalOption}
        units={property?.units ?? []}
        initialValues={
          editingRentalOption
            ? {
                name: editingRentalOption.name,
                monthlyRent: editingRentalOption.monthlyRent,
                status: editingRentalOption.status,
                unitIds: editingRentalOption.unitIds,
              }
            : undefined
        }
        isSubmitting={
          editingRentalOption
            ? updateRentalOptionMutation.isPending
            : createRentalOptionMutation.isPending
        }
        isSuccess={
          editingRentalOption
            ? updateRentalOptionMutation.isSuccess
            : createRentalOptionMutation.isSuccess
        }
      />
      <ConfirmDialog
        isOpen={!!rentalOptionPendingDelete}
        title="Delete rental option"
        message={`Are you sure you want to delete rental option, ${rentalOptionPendingDelete?.name}?`}
        confirmLabel="Delete rental option"
        onConfirm={handleConfirmDeleteRentalOption}
        onCancel={() => setRentalOptionPendingDelete(null)}
      />
    </div>
  );
}
