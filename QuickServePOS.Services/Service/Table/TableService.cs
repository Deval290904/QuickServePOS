using AutoMapper;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.RestaurantTable;
using QuickServePOS.Models.Entities.Table;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Services.IService;
using QuickServePOS.Services.IService.Table;
using QuickServePOS.Services.Service.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickServePOS.Services.Service
{
   public class TableService : ITableService
   {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TableStateMachineService _stateMachine;

        public TableService(IUnitOfWork unitOfWork, IMapper mapper, TableStateMachineService stateMachine)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stateMachine = stateMachine;
        }

        public async Task<List<TableListDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Tables.GetAllAsync();

            return _mapper.Map<List<TableListDto>>(entities);
        }

        public async Task<TableUpdateDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Tables.GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<TableUpdateDto>(entity);
        }

        public async Task<ApiResponse> CreateAsync(TableCreateDto dto)
        {
            // Check floor exists

            var floor = await _unitOfWork.Floors.GetByIdAsync(dto.FloorId);

            if (floor == null)
            {
                return Fail("Floor not found");
            }

            // Duplicate table check

            var exists = await _unitOfWork.Tables.ExistsAsync(dto.TableNumber,dto.FloorId);

            if (exists)
            {
                return Fail("Table number already exists on this floor");
            }

            var entity = _mapper.Map<RestaurantTableEntity>(dto);

            await _unitOfWork.Tables.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Table created successfully");
        }

        public async Task<ApiResponse> UpdateAsync(TableUpdateDto dto)
        {
            var entity = await _unitOfWork.Tables.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return Fail("Table not found");
            }

            // Check floor exists

            var floor = await _unitOfWork.Floors.GetByIdAsync(dto.FloorId);

            if (floor == null)
            {
                return Fail("Floor not found");
            }

            // Duplicate table validation

            var exists = await _unitOfWork.Tables.ExistsAsync(dto.TableNumber,dto.FloorId,dto.Id);

            if (exists)
            {
                return Fail("Table number already exists on this floor");
            }

            // Status validation

            if (!_stateMachine.CanMoveTo(entity.Status,dto.Status))
            {
                return Fail($"Cannot move from {entity.Status} to {dto.Status}");
            }

            _mapper.Map(dto, entity);

            _unitOfWork.Tables.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Table updated successfully");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Tables.GetByIdAsync(id);

            if (entity == null)
            {
                return Fail("Table not found");
            }

            // Prevent delete if occupied

            if (entity.Status ==Models.Entities.Enums.TableStatus.Occupied)
            {
                return Fail("Occupied table cannot be deleted");
            }

            _unitOfWork.Tables.Delete(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Table deleted successfully");
        }

        public async Task<ApiResponse> RestoreAsync(int id)
        {
            var entity = await _unitOfWork.Tables.GetByIdIgnoreQueryFilterAsync(id);

            if (entity == null)
            {
                return Fail("Table not found");
            }

            entity.IsDeleted = false;

            entity.DeletedAt = null;

            _unitOfWork.Tables.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return Success("Table restored successfully");
        }

        public async Task<List<TableListDto>> GetDeletedAsync()
        {
            var entities = await _unitOfWork.Tables.GetDeletedTablesAsync();

            return _mapper.Map<List<TableListDto>>(entities);
        }

        private ApiResponse Success(string message)
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        private ApiResponse Fail(string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message
            };
        }
    }
}
